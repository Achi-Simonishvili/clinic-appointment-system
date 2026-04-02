using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.Infrastructure.Repositories;

public class DoctorAvailabilityRepository : RepositoryBase<DoctorAvailability>, IDoctorAvailabilityRepository
{
    public DoctorAvailabilityRepository(AppDbContext context) : base(context) { }

    public async Task<List<DoctorAvailability>> GetByDoctorIdAsync(Guid doctorId) =>
        await _dbSet.Where(a => a.DoctorId == doctorId).ToListAsync();
}