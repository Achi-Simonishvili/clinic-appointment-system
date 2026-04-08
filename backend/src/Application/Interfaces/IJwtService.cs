using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(AppUser user);
}
