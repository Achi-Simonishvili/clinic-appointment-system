using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser?> GetByIdAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(AppUser user);
    Task SaveChangesAsync();
}
