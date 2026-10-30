using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointmentsApi.Data;
using AppointmentsApi.Models;
using AppointmentsApi.Services;
using Grpc.Net.Client;
using AppointmentsApi.Protos;

namespace AppointmentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentContext _context;
        private readonly PatientApiClient _patientApiClient;
        private readonly DoctorApiClient _doctorApiClient;
        private readonly IConfiguration _configuration;

    public AppointmentsController(AppointmentContext context, PatientApiClient patientApiClient,
        DoctorApiClient doctorApiClient, IConfiguration configuration)
    {
        _context = context;
        _patientApiClient = patientApiClient;
        _doctorApiClient = doctorApiClient;
        _configuration = configuration;
    }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            var patient = await _patientApiClient.GetPatientAsync(id);
            var doctor = await _doctorApiClient.GetDoctorAsync(id);

            var grpcEndpoint = _configuration["GrpcEndpoints:DocumentService"]!;
            using var channel = GrpcChannel.ForAddress(grpcEndpoint);
            var client = new DocumentService.DocumentServiceClient(channel);
            var documents = await client.GetAllAsync(new PatientId { Id = patient.PatientId.ToString() });

            AppointmentDetails appointmentDetails = new AppointmentDetails(
                appointment.AppointmentId,
                patient,
                doctor,
                appointment.Slot?.Start ?? DateTime.MinValue,
                appointment.Slot?.End ?? DateTime.MinValue,
                appointment.Location?.RoomNumber,
                appointment.Location?.Building,
                documents
            );

            return Ok(appointmentDetails);
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(Guid id, Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.AppointmentId }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(Guid id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}
