namespace ClinicSystem.Application.Interfaces;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(string toEmail, string patientName, string doctorName, DateTime appointmentDate, string startTime);
    Task SendBookingCancellationAsync(string toEmail, string patientName, string doctorName, DateTime appointmentDate, string startTime);
    Task SendReminderAsync(string toEmail, string patientName, string doctorName, DateTime appointmentDate, string startTime);
}