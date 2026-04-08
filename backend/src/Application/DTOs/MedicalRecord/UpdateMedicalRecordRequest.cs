namespace ClinicSystem.Application.DTOs.MedicalRecord;

public class UpdateMedicalRecordRequest
{
    public string Diagnosis { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}