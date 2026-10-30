using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentsApi.Messages;
using AppointmentsApi.Services;
using MassTransit;

namespace AppointmentsApi.Consumers;


public class AppointmentCreatedConsumer(IEmailService emailService,
    PatientApiClient patientApiClient, DoctorApiClient doctorApiClient) : IConsumer<AppointmentCreated>
{
    public async Task Consume(ConsumeContext<AppointmentCreated> context)
    {
        var message = context.Message;

        Console.WriteLine($"Retrieve Doctor details for DoctorId: {message.DoctorId}");

        var doctor = await doctorApiClient.GetDoctorAsync(message.DoctorId);

        Console.WriteLine($"Retrieve Patient details for PatientId: {message.PatientId}");
        Console.WriteLine($"Send email to patient");
        var patient = await patientApiClient.GetPatientAsync(message.PatientId);
        var emailContent = @$"Dear {patient.FirstName} {patient.LastName}, \n\n
            Your appointment with Dr. {doctor.FirstName} {doctor.LastName} is scheduled for {message.AppointmentDate}. \n\n
            Best regards, \n
            Health Care Management system";

        await emailService.SendEmailAsync(patient.Email, "Appointment Confirmation", emailContent);
    }
}