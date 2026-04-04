namespace ClinicSystem.Application.DTOs.MedicalRecord;

public class CreateMedicalRecordRequest
{
    public Guid PatientId { get; set; }
    public Guid AppointmentId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}