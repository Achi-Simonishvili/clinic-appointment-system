using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Doctor;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Enums;
using ClinicSystem.Application.DTOs.Common;

namespace ClinicSystem.Application.Services;

public class DoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISpecializationRepository _specializationRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public DoctorService(
        IDoctorRepository doctorRepository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ISpecializationRepository specializationRepository,
        IDepartmentRepository departmentRepository)
    {
        _doctorRepository = doctorRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _specializationRepository = specializationRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new BadRequestException("Email already in use.");

        var specialization = await _specializationRepository.GetAsync(s => s.Id == request.SpecializationId)
            ?? throw new NotFoundException("Specialization not found.");

        var department = await _departmentRepository.GetAsync(d => d.Id == request.DepartmentId)
            ?? throw new NotFoundException("Department not found.");

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
            SpecializationId = request.SpecializationId,
            Specialization = specialization,
            DepartmentId = request.DepartmentId,
            Department = department,
            PhoneNumber = request.PhoneNumber
        };

        await _doctorRepository.AddAsync(doctor);
        await _doctorRepository.SaveAsync();

        return MapToDto(doctor);
    }

    public async Task<DoctorDto> UpdateAsync(Guid id, UpdateDoctorRequest request)
    {
        var doctor = await _doctorRepository.GetAsync(
            d => d.Id == id, includeProperties: "User,Specialization,Department")
            ?? throw new NotFoundException("Doctor not found.");

        var specialization = await _specializationRepository.GetAsync(s => s.Id == request.SpecializationId)
            ?? throw new NotFoundException("Specialization not found.");

        var department = await _departmentRepository.GetAsync(d => d.Id == request.DepartmentId)
            ?? throw new NotFoundException("Department not found.");

        doctor.Bio = request.Bio;
        doctor.SpecializationId = request.SpecializationId;
        doctor.Specialization = specialization;
        doctor.DepartmentId = request.DepartmentId;
        doctor.Department = department;
        doctor.PhoneNumber = request.PhoneNumber;

        _doctorRepository.Update(doctor);
        await _doctorRepository.SaveAsync();

        return MapToDto(doctor);
    }

    public async Task<DoctorDto> GetByIdAsync(Guid id)
    {
        var doctor = await _doctorRepository.GetAsync(
            d => d.Id == id, includeProperties: "User,Specialization,Department")
            ?? throw new NotFoundException("Doctor not found.");

        return MapToDto(doctor);
    }

    public async Task<DoctorDto> GetByUserIdAsync(Guid userId)
    {
        var doctor = await _doctorRepository.GetAsync(
            d => d.UserId == userId, includeProperties: "User,Specialization,Department")
            ?? throw new NotFoundException("Doctor profile not found.");

        return MapToDto(doctor);
    }

    public async Task<List<DoctorDto>> GetAllAsync()
    {
        var (doctors, _) = await _doctorRepository.GetAllAsync(
            includeProperties: "User,Specialization,Department");
        return doctors.Select(MapToDto).ToList();
    }

    public async Task<PagedResponse<DoctorDto>> GetAllFilteredAsync(DoctorFilterRequest filter)
    {
        var (doctors, totalCount) = await _doctorRepository.GetAllAsync(
            filter: d =>
                (string.IsNullOrEmpty(filter.Search) ||
                    d.User.FirstName.Contains(filter.Search) ||
                    d.User.LastName.Contains(filter.Search) ||
                    d.User.Email.Contains(filter.Search)) &&
                (string.IsNullOrEmpty(filter.Specialization) ||
                    d.Specialization.Name.Contains(filter.Specialization)) &&
                (string.IsNullOrEmpty(filter.Department) ||
                    d.Department.Name.Contains(filter.Department)),
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            orderBy: filter.OrderBy,
            ascending: filter.Ascending,
            includeProperties: "User,Specialization,Department"
        );

        return new PagedResponse<DoctorDto>
        {
            Items = doctors.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    private static DoctorDto MapToDto(Doctor doctor) => new()
    {
        Id = doctor.Id,
        UserId = doctor.UserId,
        FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
        Email = doctor.User.Email,
        LicenseNumber = doctor.LicenseNumber,
        Bio = doctor.Bio,
        SpecializationId = doctor.SpecializationId,
        SpecializationName = doctor.Specialization.Name,
        DepartmentId = doctor.DepartmentId,
        DepartmentName = doctor.Department.Name,
        PhoneNumber = doctor.PhoneNumber,
        IsActive = doctor.IsActive
    };
}