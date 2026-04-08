namespace ClinicSystem.Application.DTOs.Appointment;

public class AppointmentFilterRequest : Common.PagedRequest
{
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}