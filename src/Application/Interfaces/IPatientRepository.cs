using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IPatientRepository : IRepositoryBase<Patient>
{
    Task<Patient?> GetByUserIdAsync(Guid userId);
}
