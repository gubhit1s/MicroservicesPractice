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
    var response = await _httpClient.GetAsync($"/api/patients/{patientId}");
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<Patient>() ?? throw new InvalidOperationException("Failed to deserialize patient data.");
  }
  
}
