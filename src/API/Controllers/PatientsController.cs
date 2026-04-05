using ClinicSystem.Application.DTOs.Patient;
using ClinicSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly PatientService _patientService;

    public PatientsController(PatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return Ok(patient);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Patient")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePatientRequest request)
    {
        var patient = await _patientService.UpdateAsync(id, request);
        return Ok(patient);
    }

    [HttpGet("{patientId}/history")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetHistory(Guid patientId)
    {
        var result = await _patientService.GetHistoryAsync(patientId);
        return Ok(result);
    }
}
