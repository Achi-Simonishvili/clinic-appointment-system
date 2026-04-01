namespace ClinicSystem.Application.DTOs.Doctor;

public class UpdateDoctorRequest
{
    public string Bio { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
