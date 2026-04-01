namespace ClinicSystem.Application.DTOs.Patient;

public class UpdatePatientRequest
{
    public DateTime DateOfBirth { get; set; }
    public string BloodType { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;
}
