namespace ClinicSystem.Application.DTOs.Patient;

public class PatientFilterRequest : Common.PagedRequest
{
    public string? Search { get; set; }
}