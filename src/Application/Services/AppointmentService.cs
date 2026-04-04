using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Appointment;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Enums;

namespace ClinicSystem.Application.Services;

public class AppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorAvailabilityRepository _availabilityRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        IDoctorAvailabilityRepository availabilityRepository)
    {
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _availabilityRepository = availabilityRepository;
    }

    public async Task<AppointmentDto> BookAsync(Guid patientId, BookAppointmentRequest request)
    {
        var patient = await _patientRepository.GetAsync(p => p.Id == patientId, includeProperties: "User")
            ?? throw new NotFoundException("Patient not found.");

        var doctor = await _doctorRepository.GetAsync(d => d.Id == request.DoctorId, includeProperties: "User")
            ?? throw new NotFoundException("Doctor not found.");

        if (!TimeOnly.TryParse(request.StartTime, out var startTime))
            throw new BadRequestException("Invalid start time. Use format HH:mm.");

        // Check doctor availability for that day of week
        var dayOfWeek = (ClinicDayOfWeek)((int)request.AppointmentDate.DayOfWeek == 0 ? 6 : (int)request.AppointmentDate.DayOfWeek - 1);
        var availability = await _availabilityRepository.GetAsync(
            a => a.DoctorId == request.DoctorId && a.DayOfWeek == dayOfWeek && a.IsAvailable)
            ?? throw new BadRequestException("Doctor is not available on that day.");

        // Check time is within working hours
        var endTime = startTime.AddMinutes(availability.SlotDurationMinutes);
        if (startTime < availability.StartTime || endTime > availability.EndTime)
            throw new BadRequestException("Requested time is outside doctor's working hours.");

        // Check for conflicts
        var hasConflict = await _appointmentRepository.HasConflictAsync(
            request.DoctorId, request.AppointmentDate, startTime, endTime);
        if (hasConflict)
            throw new BadRequestException("This time slot is already booked.");

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Patient = patient,
            DoctorId = request.DoctorId,
            Doctor = doctor,
            AppointmentDate = request.AppointmentDate,
            StartTime = startTime,
            EndTime = endTime,
            Status = AppointmentStatus.Pending
        };

        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveAsync();

        return MapToDto(appointment);
    }

    public async Task<AppointmentDto> GetByIdAsync(Guid id)
    {
        var appointment = await _appointmentRepository.GetAsync(
            a => a.Id == id, includeProperties: "Patient.User,Doctor.User")
            ?? throw new NotFoundException("Appointment not found.");

        return MapToDto(appointment);
    }

    public async Task<List<AppointmentDto>> GetByDoctorIdAsync(Guid doctorId)
    {
        var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId);
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<List<AppointmentDto>> GetByPatientIdAsync(Guid patientId)
    {
        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<AppointmentDto> UpdateStatusAsync(Guid id, UpdateAppointmentStatusRequest request)
    {
        var appointment = await _appointmentRepository.GetAsync(
            a => a.Id == id, includeProperties: "Patient.User,Doctor.User")
            ?? throw new NotFoundException("Appointment not found.");

        if (!Enum.TryParse<AppointmentStatus>(request.Status, true, out var newStatus))
            throw new BadRequestException("Invalid status. Valid values: Pending, Confirmed, Completed, Cancelled.");

        // Validate status transitions
        var validTransitions = new Dictionary<AppointmentStatus, List<AppointmentStatus>>
        {
            { AppointmentStatus.Pending, new List<AppointmentStatus> { AppointmentStatus.Confirmed, AppointmentStatus.Cancelled } },
            { AppointmentStatus.Confirmed, new List<AppointmentStatus> { AppointmentStatus.Completed, AppointmentStatus.Cancelled } },
            { AppointmentStatus.Completed, new List<AppointmentStatus>() },
            { AppointmentStatus.Cancelled, new List<AppointmentStatus>() }
        };

        if (!validTransitions[appointment.Status].Contains(newStatus))
            throw new BadRequestException($"Cannot transition from {appointment.Status} to {newStatus}.");

        appointment.Status = newStatus;
        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveAsync();

        return MapToDto(appointment);
    }

    public async Task<List<AppointmentDto>> GetAllAsync()
    {
        var (appointments, _) = await _appointmentRepository.GetAllAsync(
            includeProperties: "Patient.User,Doctor.User");
        return appointments.Select(MapToDto).ToList();
    }

    private static AppointmentDto MapToDto(Appointment a) => new()
    {
        Id = a.Id,
        PatientId = a.PatientId,
        PatientName = $"{a.Patient.User.FirstName} {a.Patient.User.LastName}",
        DoctorId = a.DoctorId,
        DoctorName = $"{a.Doctor.User.FirstName} {a.Doctor.User.LastName}",
        AppointmentDate = a.AppointmentDate,
        StartTime = a.StartTime.ToString("HH:mm"),
        EndTime = a.EndTime.ToString("HH:mm"),
        Status = a.Status.ToString(),
        Notes = a.Notes
    };
}