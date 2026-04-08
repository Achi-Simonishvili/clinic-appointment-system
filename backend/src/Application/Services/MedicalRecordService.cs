using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.MedicalRecord;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Services;

public class MedicalRecordService
{
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public MedicalRecordService(
        IMedicalRecordRepository medicalRecordRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<MedicalRecordDto> CreateAsync(Guid doctorId, CreateMedicalRecordRequest request)
    {
        var patient = await _patientRepository.GetAsync(p => p.Id == request.PatientId, includeProperties: "User")
            ?? throw new NotFoundException("Patient not found.");

        var doctor = await _doctorRepository.GetAsync(d => d.Id == doctorId, includeProperties: "User")
            ?? throw new NotFoundException("Doctor not found.");

        var appointment = await _appointmentRepository.GetAsync(a => a.Id == request.AppointmentId)
            ?? throw new NotFoundException("Appointment not found.");

        if (appointment.DoctorId != doctorId)
            throw new ForbidException("You can only create records for your own appointments.");

        if (appointment.PatientId != request.PatientId)
            throw new BadRequestException("Patient does not match the appointment.");

        var record = new MedicalRecord
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            Patient = patient,
            DoctorId = doctorId,
            Doctor = doctor,
            AppointmentId = request.AppointmentId,
            Appointment = appointment,
            Diagnosis = request.Diagnosis,
            Symptoms = request.Symptoms,
            Notes = request.Notes
        };

        await _medicalRecordRepository.AddAsync(record);
        await _medicalRecordRepository.SaveAsync();

        return MapToDto(record);
    }

    public async Task<MedicalRecordDto> UpdateAsync(Guid id, UpdateMedicalRecordRequest request)
    {
        var record = await _medicalRecordRepository.GetAsync(
            r => r.Id == id, includeProperties: "Patient.User,Doctor.User")
            ?? throw new NotFoundException("Medical record not found.");

        record.Diagnosis = request.Diagnosis;
        record.Symptoms = request.Symptoms;
        record.Notes = request.Notes;
        record.UpdatedAt = DateTime.UtcNow;

        _medicalRecordRepository.Update(record);
        await _medicalRecordRepository.SaveAsync();

        return MapToDto(record);
    }

    public async Task<MedicalRecordDto> GetByIdAsync(Guid id)
    {
        var record = await _medicalRecordRepository.GetAsync(
            r => r.Id == id, includeProperties: "Patient.User,Doctor.User")
            ?? throw new NotFoundException("Medical record not found.");

        return MapToDto(record);
    }

    public async Task<List<MedicalRecordDto>> GetByPatientIdAsync(Guid patientId)
    {
        var records = await _medicalRecordRepository.GetByPatientIdAsync(patientId);
        return records.Select(MapToDto).ToList();
    }

    public async Task<List<MedicalRecordDto>> GetByDoctorIdAsync(Guid doctorId)
    {
        var records = await _medicalRecordRepository.GetByDoctorIdAsync(doctorId);
        return records.Select(MapToDto).ToList();
    }

    private static MedicalRecordDto MapToDto(MedicalRecord r) => new()
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
    };
}