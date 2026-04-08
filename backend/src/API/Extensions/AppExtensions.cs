using ClinicSystem.Application.Interfaces;
using ClinicSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.API.Extensions;

public static class AppExtensions
{
    public static void UseDbAutoUpdate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        db.Database.Migrate();
        DataSeeder.SeedAsync(db, hasher).Wait();
    }
}