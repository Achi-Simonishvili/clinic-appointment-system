using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Availability;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Enums;

namespace ClinicSystem.Application.Services;

public class DoctorAvailabilityService
{
    private readonly IDoctorAvailabilityRepository _availabilityRepository;
    private readonly IDoctorRepository _doctorRepository;

    public DoctorAvailabilityService(
        IDoctorAvailabilityRepository availabilityRepository,
        IDoctorRepository doctorRepository)
    {
        _availabilityRepository = availabilityRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<DoctorAvailabilityDto> SetAvailabilityAsync(Guid doctorId, SetAvailabilityRequest request)
    {
        var doctor = await _doctorRepository.GetAsync(d => d.Id == doctorId)
            ?? throw new NotFoundException("Doctor not found.");

        if (!Enum.TryParse<ClinicDayOfWeek>(request.DayOfWeek, true, out var day))
            throw new BadRequestException("Invalid day of week.");

        if (!TimeOnly.TryParse(request.StartTime, out var startTime))
            throw new BadRequestException("Invalid start time. Use format HH:mm.");

        if (!TimeOnly.TryParse(request.EndTime, out var endTime))
            throw new BadRequestException("Invalid end time. Use format HH:mm.");

        if (endTime <= startTime)
            throw new BadRequestException("End time must be after start time.");

        if (request.SlotDurationMinutes <= 0)
            throw new BadRequestException("Slot duration must be greater than 0.");

        var existing = await _availabilityRepository.GetAsync(
            a => a.DoctorId == doctorId && a.DayOfWeek == day);

        if (existing != null)
        {
            existing.StartTime = startTime;
            existing.EndTime = endTime;
            existing.SlotDurationMinutes = request.SlotDurationMinutes;
            existing.IsAvailable = true;
            _availabilityRepository.Update(existing);
            await _availabilityRepository.SaveAsync();
            return MapToDto(existing);
        }

        var availability = new DoctorAvailability
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorId,
            Doctor = doctor,
            DayOfWeek = day,
            StartTime = startTime,
            EndTime = endTime,
            SlotDurationMinutes = request.SlotDurationMinutes
        };

        await _availabilityRepository.AddAsync(availability);
        await _availabilityRepository.SaveAsync();

        return MapToDto(availability);
    }

    public async Task<List<DoctorAvailabilityDto>> GetByDoctorIdAsync(Guid doctorId)
    {
        var availabilities = await _availabilityRepository.GetByDoctorIdAsync(doctorId);
        return availabilities.Select(MapToDto).ToList();
    }

    public async Task DeleteAsync(Guid id)
    {
        var availability = await _availabilityRepository.GetAsync(a => a.Id == id)
            ?? throw new NotFoundException("Availability not found.");

        _availabilityRepository.Remove(availability);
        await _availabilityRepository.SaveAsync();
    }

    private static DoctorAvailabilityDto MapToDto(DoctorAvailability a) => new()
    {
        Id = a.Id,
        DoctorId = a.DoctorId,
        DayOfWeek = a.DayOfWeek.ToString(),
        StartTime = a.StartTime.ToString("HH:mm"),
        EndTime = a.EndTime.ToString("HH:mm"),
        SlotDurationMinutes = a.SlotDurationMinutes,
        IsAvailable = a.IsAvailable
    };
}