namespace ClinicSystem.Domain.Entities;

public class Doctor
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string LicenseNumber { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
