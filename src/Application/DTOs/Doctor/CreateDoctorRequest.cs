namespace ClinicSystem.Application.DTOs.Doctor;

public class CreateDoctorRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public Guid SpecializationId { get; set; }
    public Guid DepartmentId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}