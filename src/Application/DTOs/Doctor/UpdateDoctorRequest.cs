namespace ClinicSystem.Application.DTOs.Doctor;

public class UpdateDoctorRequest
{
    public string Bio { get; set; } = string.Empty;
    public Guid SpecializationId { get; set; }
    public Guid DepartmentId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}