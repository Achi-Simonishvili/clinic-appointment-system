using ClinicSystem.Domain.Enums;

namespace ClinicSystem.Domain.Entities;

public class DoctorAvailability
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public ClinicDayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int SlotDurationMinutes { get; set; } = 30;
    public bool IsAvailable { get; set; } = true;
}