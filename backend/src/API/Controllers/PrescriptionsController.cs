using ClinicSystem.Application.DTOs.Prescription;
using ClinicSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrescriptionsController : ControllerBase
{
    private readonly PrescriptionService _prescriptionService;

    public PrescriptionsController(PrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _prescriptionService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var result = await _prescriptionService.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    [HttpGet("medical-record/{medicalRecordId}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetByMedicalRecordId(Guid medicalRecordId)
    {
        var result = await _prescriptionService.GetByMedicalRecordIdAsync(medicalRecordId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Create([FromBody] CreatePrescriptionRequest request, [FromQuery] Guid doctorId)
    {
        var result = await _prescriptionService.CreateAsync(doctorId, request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}