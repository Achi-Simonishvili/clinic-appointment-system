namespace ClinicSystem.Application.DTOs.Prescription;

public class CreatePrescriptionRequest
{
    public Guid PatientId { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public string Instructions { get; set; } = string.Empty;
}