using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IDoctorRepository : IRepositoryBase<Doctor>
{
    Task<Doctor?> GetByUserIdAsync(Guid userId);
}
