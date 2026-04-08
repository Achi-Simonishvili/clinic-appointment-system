using ClinicSystem.Application.DTOs.Appointment;
using ClinicSystem.Application.DTOs.MedicalRecord;
using ClinicSystem.Application.DTOs.Prescription;

namespace ClinicSystem.Application.DTOs.Patient;

public class PatientHistoryDto
{
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public List<AppointmentDto> Appointments { get; set; } = new();
    public List<MedicalRecordDto> MedicalRecords { get; set; } = new();
    public List<PrescriptionDto> Prescriptions { get; set; } = new();
}