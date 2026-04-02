using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IDoctorAvailabilityRepository : IRepositoryBase<DoctorAvailability>
{
    Task<List<DoctorAvailability>> GetByDoctorIdAsync(Guid doctorId);
}