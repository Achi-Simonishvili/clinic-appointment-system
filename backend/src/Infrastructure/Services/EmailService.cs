using ClinicSystem.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ClinicSystem.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendBookingConfirmationAsync(string toEmail, string patientName, string doctorName, DateTime appointmentDate, string startTime)
    {
        var subject = "Appointment Booking Confirmation";
        var body = $"""
            Dear {patientName},

            Your appointment has been successfully booked.

            Doctor: {doctorName}
            Date: {appointmentDate:MMMM dd, yyyy}
            Time: {startTime}

            Please arrive 10 minutes early.

            Clinic System
            """;

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendBookingCancellationAsync(string toEmail, string patientName, string doctorName, DateTime appointmentDate, string startTime)
    {
        var subject = "Appointment Cancellation";
        var body = $"""
            Dear {patientName},

            Your appointment has been cancelled.

            Doctor: {doctorName}
            Date: {appointmentDate:MMMM dd, yyyy}
            Time: {startTime}

            Please contact us to reschedule.

            Clinic System
            """;

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendReminderAsync(string toEmail, string patientName, string doctorName, DateTime appointmentDate, string startTime)
    {
        var subject = "Appointment Reminder";
        var body = $"""
            Dear {patientName},

            This is a reminder for your appointment tomorrow.

            Doctor: {doctorName}
            Date: {appointmentDate:MMMM dd, yyyy}
            Time: {startTime}

            Please arrive 10 minutes early.

            Clinic System
            """;

        await SendEmailAsync(toEmail, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var host = _configuration["Email:Host"];
        var port = int.Parse(_configuration["Email:Port"]!);
        var username = _configuration["Email:Username"]!;
        var password = _configuration["Email:Password"]!;
        var fromEmail = _configuration["Email:From"]!;
        var fromName = _configuration["Email:FromName"]!;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(new MailboxAddress(toEmail, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(username, password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}