using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IAppointmentRepository : IRepositoryBase<Appointment>
{
    Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId);
    Task<List<Appointment>> GetByPatientIdAsync(Guid patientId);
    Task<bool> HasConflictAsync(Guid doctorId, DateTime date, TimeOnly startTime, TimeOnly endTime);
}