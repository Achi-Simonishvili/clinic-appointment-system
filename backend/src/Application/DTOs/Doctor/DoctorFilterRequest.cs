namespace ClinicSystem.Application.DTOs.Doctor;

public class DoctorFilterRequest : Common.PagedRequest
{
    public string? Search { get; set; }
    public string? Specialization { get; set; }
    public string? Department { get; set; }
}