using AppointmentsApi.Services;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsApi.Models;

[Owned]
public record TimeSlot(DateTime Start, DateTime End);

[Owned]
public record Location(string RoomNumber, string Building);

public class Appointment
{
  public Guid AppointmentId { get; set; }

  public Guid PatientId { get; set; }

  public Guid DoctorId { get; set; }

  public TimeSlot? Slot { get; set; }

  public Location? Location { get; set; }

  public string? Purpose { get; set; }

  public void Reschedule(TimeSlot newSlot)
  {
    Slot = newSlot;
  }

  public void ChangePurpose(string newPurpose)
  {
    Purpose = newPurpose;
  }
}


public class AppointmentDetails : Appointment
{
  public Patient? Patient { get; set; }

  public Doctor? Doctor { get; set; }

  public AppointmentDetails(Guid appointmentId, Patient? patient, Doctor? doctor,
                          DateTime slotStart, DateTime slotEnd, string? locationRoomNumber, string? locationBuilding)
      : base()
  {
    AppointmentId = appointmentId;
    Patient = patient;
    Doctor = doctor;
    Slot = new TimeSlot(slotStart, slotEnd);
    Location = new Location(locationRoomNumber ?? string.Empty, locationBuilding ?? string.Empty);
  }
}