using System;

namespace AppointmentsApi.Services;

public record Patient
(
  Guid PatientId,
  string FirstName,
  string LastName,
  DateTime DateOfBirth,
  string Gender,
  string ContactNumber,
  string Email
);

public class PatientApiClient
{
  private readonly HttpClient _httpClient;

  public PatientApiClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<Patient> GetPatientAsync(Guid patientId)
  {
    return new Patient(patientId, "Nancy", "Smith", new DateTime(1990, 5, 15), "Female", "123-456-7890", "nancy@email.com");
  }

}
