using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Patient;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Services;

public class PatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
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

    public async Task<List<PatientDto>> GetAllAsync()
    {
        var (patients, _) = await _patientRepository.GetAllAsync(includeProperties: "User");
        return patients.Select(MapToDto).ToList();
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
