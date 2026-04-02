using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Infrastructure.Persistence;

namespace ClinicSystem.Infrastructure.Repositories;

public class SpecializationRepository : RepositoryBase<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(AppDbContext context) : base(context) { }
}