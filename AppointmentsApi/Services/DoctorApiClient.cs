using System;

namespace AppointmentsApi.Services;

public record Doctor(
  Guid DoctorId,
  string FirstName,
  string LastName,
  string Specialty
);

public class DoctorApiClient
{
  private readonly HttpClient _httpClient;

  public DoctorApiClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<Doctor> GetDoctorAsync(Guid doctorId)
  {
    var response = await _httpClient.GetAsync($"/api/doctors/{doctorId}");
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<Doctor>() ?? throw new InvalidOperationException("Failed to deserialize doctor data.");
  }
}
