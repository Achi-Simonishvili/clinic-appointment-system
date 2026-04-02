using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Department;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Services;

public class DepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentRequest request)
    {
        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        await _departmentRepository.AddAsync(department);
        await _departmentRepository.SaveAsync();

        return MapToDto(department);
    }

    public async Task<DepartmentDto> UpdateAsync(Guid id, UpdateDepartmentRequest request)
    {
        var department = await _departmentRepository.GetAsync(d => d.Id == id)
            ?? throw new NotFoundException("Department not found.");

        department.Name = request.Name;
        department.Description = request.Description;

        _departmentRepository.Update(department);
        await _departmentRepository.SaveAsync();

        return MapToDto(department);
    }

    public async Task DeleteAsync(Guid id)
    {
        var department = await _departmentRepository.GetAsync(d => d.Id == id)
            ?? throw new NotFoundException("Department not found.");

        _departmentRepository.Remove(department);
        await _departmentRepository.SaveAsync();
    }

    public async Task<DepartmentDto> GetByIdAsync(Guid id)
    {
        var department = await _departmentRepository.GetAsync(d => d.Id == id)
            ?? throw new NotFoundException("Department not found.");

        return MapToDto(department);
    }

    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        var (departments, _) = await _departmentRepository.GetAllAsync();
        return departments.Select(MapToDto).ToList();
    }

    private static DepartmentDto MapToDto(Department d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Description = d.Description,
        IsActive = d.IsActive
    };
}