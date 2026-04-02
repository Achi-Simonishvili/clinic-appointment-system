using ClinicSystem.Application.DTOs.Availability;
using ClinicSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.API.Controllers;

[ApiController]
[Route("api/doctors/{doctorId}/availability")]
[Authorize]
public class DoctorAvailabilityController : ControllerBase
{
    private readonly DoctorAvailabilityService _availabilityService;

    public DoctorAvailabilityController(DoctorAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetByDoctorId(Guid doctorId)
    {
        var result = await _availabilityService.GetByDoctorIdAsync(doctorId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> SetAvailability(Guid doctorId, [FromBody] SetAvailabilityRequest request)
    {
        var result = await _availabilityService.SetAvailabilityAsync(doctorId, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Delete(Guid doctorId, Guid id)
    {
        await _availabilityService.DeleteAsync(id);
        return NoContent();
    }
}