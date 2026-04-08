using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IMedicalRecordRepository : IRepositoryBase<MedicalRecord>
{
    Task<List<MedicalRecord>> GetByPatientIdAsync(Guid patientId);
    Task<List<MedicalRecord>> GetByDoctorIdAsync(Guid doctorId);
}