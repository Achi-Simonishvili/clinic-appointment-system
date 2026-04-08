using ClinicSystem.Application.DTOs.MedicalRecord;
using ClinicSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicalRecordsController : ControllerBase
{
    private readonly MedicalRecordService _medicalRecordService;

    public MedicalRecordsController(MedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _medicalRecordService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var result = await _medicalRecordService.GetByPatientIdAsync(patientId);
        return Ok(result);
    }

    [HttpGet("doctor/{doctorId}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetByDoctorId(Guid doctorId)
    {
        var result = await _medicalRecordService.GetByDoctorIdAsync(doctorId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Create([FromBody] CreateMedicalRecordRequest request, [FromQuery] Guid doctorId)
    {
        var result = await _medicalRecordService.CreateAsync(doctorId, request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMedicalRecordRequest request)
    {
        var result = await _medicalRecordService.UpdateAsync(id, request);
        return Ok(result);
    }
}