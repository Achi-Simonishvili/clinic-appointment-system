using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IPrescriptionRepository : IRepositoryBase<Prescription>
{
    Task<List<Prescription>> GetByPatientIdAsync(Guid patientId);
    Task<List<Prescription>> GetByMedicalRecordIdAsync(Guid medicalRecordId);
}