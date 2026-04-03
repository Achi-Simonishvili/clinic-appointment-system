using ClinicSystem.Application.DTOs.Appointment;
using ClinicSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly AppointmentService _appointmentService;

    public AppointmentsController(AppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Book([FromBody] BookAppointmentRequest request, [FromQuery] Guid patientId)
    {
        var result = await _appointmentService.BookAsync(patientId, request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _appointmentService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("doctor/{doctorId}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetByDoctorId(Guid doctorId)
    {
        var result = await _appointmentService.GetByDoctorIdAsync(doctorId);
        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Admin,Patient")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var result = await _appointmentService.GetByPatientIdAsync(patientId);
        return Ok(result);
    }
}