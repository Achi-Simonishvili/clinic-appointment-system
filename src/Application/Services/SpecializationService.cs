using ClinicSystem.Application.Common.Exceptions;
using ClinicSystem.Application.DTOs.Specialization;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;

namespace ClinicSystem.Application.Services;

public class SpecializationService
{
    private readonly ISpecializationRepository _specializationRepository;

    public SpecializationService(ISpecializationRepository specializationRepository)
    {
        _specializationRepository = specializationRepository;
    }

    public async Task<SpecializationDto> CreateAsync(CreateSpecializationRequest request)
    {
        var specialization = new Specialization
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        await _specializationRepository.AddAsync(specialization);
        await _specializationRepository.SaveAsync();

        return MapToDto(specialization);
    }

    public async Task<SpecializationDto> UpdateAsync(Guid id, UpdateSpecializationRequest request)
    {
        var specialization = await _specializationRepository.GetAsync(s => s.Id == id)
            ?? throw new NotFoundException("Specialization not found.");

        specialization.Name = request.Name;
        specialization.Description = request.Description;

        _specializationRepository.Update(specialization);
        await _specializationRepository.SaveAsync();

        return MapToDto(specialization);
    }

    public async Task DeleteAsync(Guid id)
    {
        var specialization = await _specializationRepository.GetAsync(s => s.Id == id)
            ?? throw new NotFoundException("Specialization not found.");

        _specializationRepository.Remove(specialization);
        await _specializationRepository.SaveAsync();
    }

    public async Task<SpecializationDto> GetByIdAsync(Guid id)
    {
        var specialization = await _specializationRepository.GetAsync(s => s.Id == id)
            ?? throw new NotFoundException("Specialization not found.");

        return MapToDto(specialization);
    }

    public async Task<List<SpecializationDto>> GetAllAsync()
    {
        var (specializations, _) = await _specializationRepository.GetAllAsync();
        return specializations.Select(MapToDto).ToList();
    }

    private static SpecializationDto MapToDto(Specialization s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Description = s.Description,
        IsActive = s.IsActive
    };
}