using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Enums;
using ClinicSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.Infrastructure.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppDbContext context) : base(context) { }

    public async Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId) =>
        await _dbSet
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Where(a => a.DoctorId == doctorId)
            .ToListAsync();

    public async Task<List<Appointment>> GetByPatientIdAsync(Guid patientId) =>
        await _dbSet
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Where(a => a.PatientId == patientId)
            .ToListAsync();

    public async Task<bool> HasConflictAsync(Guid doctorId, DateTime date, TimeOnly startTime, TimeOnly endTime) =>
        await _dbSet.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.AppointmentDate.Date == date.Date &&
            a.Status != AppointmentStatus.Cancelled &&
            a.StartTime < endTime &&
            a.EndTime > startTime);
}