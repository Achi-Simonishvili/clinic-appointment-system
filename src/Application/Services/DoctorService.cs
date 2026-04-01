using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Doctor;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Enums;

namespace ClinicSystem.Application.Services;

public class DoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public DoctorService(IDoctorRepository doctorRepository, IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _doctorRepository = doctorRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new BadRequestException("Email already in use.");

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = UserRole.Doctor.ToString()
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveAsync();

        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            LicenseNumber = request.LicenseNumber,
            Bio = request.Bio,
            Specialization = request.Specialization,
            Department = request.Department,
            PhoneNumber = request.PhoneNumber
        };

        await _doctorRepository.AddAsync(doctor);
        await _doctorRepository.SaveAsync();

        return MapToDto(doctor);
    }

    public async Task<DoctorDto> UpdateAsync(Guid id, UpdateDoctorRequest request)
    {
        var doctor = await _doctorRepository.GetAsync(d => d.Id == id, includeProperties: "User")
            ?? throw new NotFoundException("Doctor not found.");

        doctor.Bio = request.Bio;
        doctor.Specialization = request.Specialization;
        doctor.Department = request.Department;
        doctor.PhoneNumber = request.PhoneNumber;

        _doctorRepository.Update(doctor);
        await _doctorRepository.SaveAsync();

        return MapToDto(doctor);
    }

    public async Task<DoctorDto> GetByIdAsync(Guid id)
    {
        var doctor = await _doctorRepository.GetAsync(d => d.Id == id, includeProperties: "User")
            ?? throw new NotFoundException("Doctor not found.");

        return MapToDto(doctor);
    }

    public async Task<List<DoctorDto>> GetAllAsync()
    {
        var (doctors, _) = await _doctorRepository.GetAllAsync(includeProperties: "User");
        return doctors.Select(MapToDto).ToList();
    }

    private static DoctorDto MapToDto(Doctor doctor) => new()
    {
        Id = doctor.Id,
        UserId = doctor.UserId,
        FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
        Email = doctor.User.Email,
        LicenseNumber = doctor.LicenseNumber,
        Bio = doctor.Bio,
        Specialization = doctor.Specialization,
        Department = doctor.Department,
        PhoneNumber = doctor.PhoneNumber,
        IsActive = doctor.IsActive
    };
}
