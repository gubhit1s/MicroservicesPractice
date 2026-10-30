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
    return new Doctor(doctorId, "John", "Doe", "Cardiology");
  }
}
