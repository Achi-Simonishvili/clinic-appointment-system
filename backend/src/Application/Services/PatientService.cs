using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Appointment;
using ClinicSystem.Application.DTOs.MedicalRecord;
using ClinicSystem.Application.DTOs.Patient;
using ClinicSystem.Application.DTOs.Prescription;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Application.DTOs.Common;

namespace ClinicSystem.Application.Services;

public class PatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly IPrescriptionRepository _prescriptionRepository;

    public PatientService(
        IPatientRepository patientRepository,
        IAppointmentRepository appointmentRepository,
        IMedicalRecordRepository medicalRecordRepository,
        IPrescriptionRepository prescriptionRepository)
    {
        _patientRepository = patientRepository;
        _appointmentRepository = appointmentRepository;
        _medicalRecordRepository = medicalRecordRepository;
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<PatientDto> UpdateAsync(Guid id, UpdatePatientRequest request)
    {
        var patient = await _patientRepository.GetAsync(p => p.Id == id, includeProperties: "User")
            ?? throw new NotFoundException("Patient not found.");

        patient.DateOfBirth = request.DateOfBirth;
        patient.BloodType = request.BloodType;
        patient.Address = request.Address;
        patient.PhoneNumber = request.PhoneNumber;
        patient.EmergencyContactName = request.EmergencyContactName;
        patient.EmergencyContactPhone = request.EmergencyContactPhone;

        _patientRepository.Update(patient);
        await _patientRepository.SaveAsync();

        return MapToDto(patient);
    }

    public async Task<PatientDto> GetByIdAsync(Guid id)
    {
        var patient = await _patientRepository.GetAsync(p => p.Id == id, includeProperties: "User")
            ?? throw new NotFoundException("Patient not found.");

        return MapToDto(patient);
    }

    public async Task<PatientDto> GetByUserIdAsync(Guid userId)
    {
        var patient = await _patientRepository.GetAsync(
            p => p.UserId == userId, includeProperties: "User")
            ?? throw new NotFoundException("Patient profile not found.");

        return MapToDto(patient);
    }

    public async Task<List<PatientDto>> GetAllAsync()
    {
        var (patients, _) = await _patientRepository.GetAllAsync(includeProperties: "User");
        return patients.Select(MapToDto).ToList();
    }

    public async Task<PatientHistoryDto> GetHistoryAsync(Guid patientId)
    {
        var patient = await _patientRepository.GetAsync(p => p.Id == patientId, includeProperties: "User")
            ?? throw new NotFoundException("Patient not found.");

        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
        var medicalRecords = await _medicalRecordRepository.GetByPatientIdAsync(patientId);
        var prescriptions = await _prescriptionRepository.GetByPatientIdAsync(patientId);

        return new PatientHistoryDto
        {
            PatientId = patientId,
            PatientName = $"{patient.User.FirstName} {patient.User.LastName}",
            Appointments = appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = $"{a.Patient.User.FirstName} {a.Patient.User.LastName}",
                DoctorId = a.DoctorId,
                DoctorName = $"{a.Doctor.User.FirstName} {a.Doctor.User.LastName}",
                AppointmentDate = a.AppointmentDate,
                StartTime = a.StartTime.ToString("HH:mm"),
                EndTime = a.EndTime.ToString("HH:mm"),
                Status = a.Status.ToString(),
                Notes = a.Notes
            }).OrderByDescending(a => a.AppointmentDate).ToList(),
            MedicalRecords = medicalRecords.Select(r => new MedicalRecordDto
            {
                Id = r.Id,
                PatientId = r.PatientId,
                PatientName = $"{r.Patient.User.FirstName} {r.Patient.User.LastName}",
                DoctorId = r.DoctorId,
                DoctorName = $"{r.Doctor.User.FirstName} {r.Doctor.User.LastName}",
                AppointmentId = r.AppointmentId,
                Diagnosis = r.Diagnosis,
                Symptoms = r.Symptoms,
                Notes = r.Notes,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList(),
            Prescriptions = prescriptions.Select(p => new PrescriptionDto
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
            }).ToList()
        };
    }

    public async Task<PagedResponse<PatientDto>> GetAllFilteredAsync(PatientFilterRequest filter)
    {
        var (patients, totalCount) = await _patientRepository.GetAllAsync(
            filter: p =>
                string.IsNullOrEmpty(filter.Search) ||
                p.User.FirstName.Contains(filter.Search) ||
                p.User.LastName.Contains(filter.Search) ||
                p.User.Email.Contains(filter.Search),
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            orderBy: filter.OrderBy,
            ascending: filter.Ascending,
            includeProperties: "User"
        );

        return new PagedResponse<PatientDto>
        {
            Items = patients.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    private static PatientDto MapToDto(Patient patient) => new()
    {
        Id = patient.Id,
        UserId = patient.UserId,
        FullName = $"{patient.User.FirstName} {patient.User.LastName}",
        Email = patient.User.Email,
        DateOfBirth = patient.DateOfBirth,
        BloodType = patient.BloodType,
        Address = patient.Address,
        PhoneNumber = patient.PhoneNumber,
        EmergencyContactName = patient.EmergencyContactName,
        EmergencyContactPhone = patient.EmergencyContactPhone,
        IsActive = patient.IsActive
    };
}