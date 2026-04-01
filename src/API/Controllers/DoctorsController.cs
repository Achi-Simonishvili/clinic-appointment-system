using ClinicSystem.Application.DTOs.Doctor;
using ClinicSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DoctorsController : ControllerBase
{
    private readonly DoctorService _doctorService;

    public DoctorsController(DoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _doctorService.GetAllAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        return Ok(doctor);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateDoctorRequest request)
    {
        var doctor = await _doctorService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDoctorRequest request)
    {
        var doctor = await _doctorService.UpdateAsync(id, request);
        return Ok(doctor);
    }
}
