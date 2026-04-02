namespace ClinicSystem.Application.DTOs.Availability;

public class DoctorAvailabilityDto
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public int SlotDurationMinutes { get; set; }
    public bool IsAvailable { get; set; }
}