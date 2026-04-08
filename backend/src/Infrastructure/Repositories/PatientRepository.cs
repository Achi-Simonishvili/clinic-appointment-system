using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.Infrastructure.Repositories;

public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
{
    public PatientRepository(AppDbContext context) : base(context) { }

    public async Task<Patient?> GetByUserIdAsync(Guid userId) =>
        await _dbSet.Include(p => p.User).FirstOrDefaultAsync(p => p.UserId == userId);
}
