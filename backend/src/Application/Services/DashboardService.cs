using ClinicSystem.Application.DTOs.Dashboard;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Enums;

namespace ClinicSystem.Application.Services;

public class DashboardService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly IPrescriptionRepository _prescriptionRepository;

    public DashboardService(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        IMedicalRecordRepository medicalRecordRepository,
        IPrescriptionRepository prescriptionRepository)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
        _medicalRecordRepository = medicalRecordRepository;
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var today = DateTime.UtcNow.Date;

        var (patients, totalPatients) = await _patientRepository.GetAllAsync();
        var (doctors, totalDoctors) = await _doctorRepository.GetAllAsync();
        var (appointments, totalAppointments) = await _appointmentRepository.GetAllAsync();
        var (medicalRecords, totalMedicalRecords) = await _medicalRecordRepository.GetAllAsync();
        var (prescriptions, totalPrescriptions) = await _prescriptionRepository.GetAllAsync();

        return new DashboardStatsDto
        {
            TotalPatients = totalPatients,
            TotalDoctors = totalDoctors,
            TotalAppointments = totalAppointments,
            AppointmentsToday = appointments.Count(a => a.AppointmentDate.Date == today),
            PendingAppointments = appointments.Count(a => a.Status == AppointmentStatus.Pending),
            ConfirmedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Confirmed),
            CompletedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Completed),
            CancelledAppointments = appointments.Count(a => a.Status == AppointmentStatus.Cancelled),
            TotalMedicalRecords = totalMedicalRecords,
            TotalPrescriptions = totalPrescriptions
        };
    }
}