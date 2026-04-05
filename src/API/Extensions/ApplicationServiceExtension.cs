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
        services.AddScoped<SpecializationService>();
        services.AddScoped<DepartmentService>();
        services.AddScoped<DoctorAvailabilityService>();
        services.AddScoped<AppointmentService>();
        services.AddScoped<MedicalRecordService>();
        services.AddScoped<PrescriptionService>();

        return services;
    }
}