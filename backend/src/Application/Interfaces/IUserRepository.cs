using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IUserRepository : IRepositoryBase<AppUser>
{
    Task<AppUser?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
}
