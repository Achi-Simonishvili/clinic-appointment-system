using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Prescription;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Services;

public class PrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMedicalRecordRepository _medicalRecordRepository;

    public PrescriptionService(
        IPrescriptionRepository prescriptionRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IMedicalRecordRepository medicalRecordRepository)
    {
        _prescriptionRepository = prescriptionRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _medicalRecordRepository = medicalRecordRepository;
    }

    public async Task<PrescriptionDto> CreateAsync(Guid doctorId, CreatePrescriptionRequest request)
    {
        var patient = await _patientRepository.GetAsync(p => p.Id == request.PatientId, includeProperties: "User")
            ?? throw new NotFoundException("Patient not found.");

        var doctor = await _doctorRepository.GetAsync(d => d.Id == doctorId, includeProperties: "User")
            ?? throw new NotFoundException("Doctor not found.");

        var medicalRecord = await _medicalRecordRepository.GetAsync(m => m.Id == request.MedicalRecordId)
            ?? throw new NotFoundException("Medical record not found.");

        if (medicalRecord.DoctorId != doctorId)
            throw new ForbidException("You can only create prescriptions for your own medical records.");

        if (medicalRecord.PatientId != request.PatientId)
            throw new BadRequestException("Patient does not match the medical record.");

        var prescription = new Prescription
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            Patient = patient,
            DoctorId = doctorId,
            Doctor = doctor,
            MedicalRecordId = request.MedicalRecordId,
            MedicalRecord = medicalRecord,
            MedicationName = request.MedicationName,
            Dosage = request.Dosage,
            Frequency = request.Frequency,
            DurationDays = request.DurationDays,
            Instructions = request.Instructions
        };

        await _prescriptionRepository.AddAsync(prescription);
        await _prescriptionRepository.SaveAsync();

        return MapToDto(prescription);
    }

    public async Task<PrescriptionDto> GetByIdAsync(Guid id)
    {
        var prescription = await _prescriptionRepository.GetAsync(
            p => p.Id == id, includeProperties: "Patient.User,Doctor.User")
            ?? throw new NotFoundException("Prescription not found.");

        return MapToDto(prescription);
    }

    public async Task<List<PrescriptionDto>> GetByPatientIdAsync(Guid patientId)
    {
        var prescriptions = await _prescriptionRepository.GetByPatientIdAsync(patientId);
        return prescriptions.Select(MapToDto).ToList();
    }

    public async Task<List<PrescriptionDto>> GetByMedicalRecordIdAsync(Guid medicalRecordId)
    {
        var prescriptions = await _prescriptionRepository.GetByMedicalRecordIdAsync(medicalRecordId);
        return prescriptions.Select(MapToDto).ToList();
    }

    private static PrescriptionDto MapToDto(Prescription p) => new()
    {
        Id = p.Id,
        PatientId = p.PatientId,
        PatientName = $"{p.Patient.User.FirstName} {p.Patient.User.LastName}",
        DoctorId = p.DoctorId,
        DoctorName = $"{p.Doctor.User.FirstName} {p.Doctor.User.LastName}",
        MedicalRecordId = p.MedicalRecordId,
        MedicationName = p.MedicationName,
        Dosage = p.Dosage,
        Frequency = p.Frequency,
        DurationDays = p.DurationDays,
        Instructions = p.Instructions,
        CreatedAt = p.CreatedAt
    };
}