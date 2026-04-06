using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.Infrastructure.Repositories;

public class MedicalRecordRepository : RepositoryBase<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(AppDbContext context) : base(context) { }

    public async Task<List<MedicalRecord>> GetByPatientIdAsync(Guid patientId) =>
        await _dbSet
            .Include(r => r.Patient).ThenInclude(p => p.User)
            .Include(r => r.Doctor).ThenInclude(d => d.User)
            .Where(r => r.PatientId == patientId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<List<MedicalRecord>> GetByDoctorIdAsync(Guid doctorId) =>
        await _dbSet
            .Include(r => r.Patient).ThenInclude(p => p.User)
            .Include(r => r.Doctor).ThenInclude(d => d.User)
            .Where(r => r.DoctorId == doctorId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
}