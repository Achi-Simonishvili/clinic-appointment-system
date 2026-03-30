using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<AppUser?> GetByIdAsync(Guid id) =>
        await _context.Users.FindAsync(id);

    public async Task<bool> EmailExistsAsync(string email) =>
        await _context.Users.AnyAsync(u => u.Email == email);

    public async Task AddAsync(AppUser user) =>
        await _context.Users.AddAsync(user);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
