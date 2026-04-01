using ClinicSystem.Application.Services;

namespace ClinicSystem.API.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<DoctorService>();
        services.AddScoped<PatientService>();

        return services;
    }
}