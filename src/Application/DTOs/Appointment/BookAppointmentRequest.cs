namespace ClinicSystem.Application.DTOs.Appointment;

public class BookAppointmentRequest
{
    public Guid DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string StartTime { get; set; } = string.Empty;
}