using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.Infrastructure.Repositories;

public class PrescriptionRepository : RepositoryBase<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(AppDbContext context) : base(context) { }

    public async Task<List<Prescription>> GetByPatientIdAsync(Guid patientId) =>
        await _dbSet
            .Include(p => p.Patient).ThenInclude(p => p.User)
            .Include(p => p.Doctor).ThenInclude(d => d.User)
            .Where(p => p.PatientId == patientId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<List<Prescription>> GetByMedicalRecordIdAsync(Guid medicalRecordId) =>
        await _dbSet
            .Include(p => p.Patient).ThenInclude(p => p.User)
            .Include(p => p.Doctor).ThenInclude(d => d.User)
            .Where(p => p.MedicalRecordId == medicalRecordId)
            .ToListAsync();
}