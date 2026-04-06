using ClinicSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Specialization> Specializations => Set<Specialization>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<DoctorAvailability> DoctorAvailabilities => Set<DoctorAvailability>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Role).IsRequired();
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithOne()
                  .HasForeignKey<Doctor>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Specialization)
                  .WithMany(s => s.Doctors)
                  .HasForeignKey(e => e.SpecializationId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Doctors)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Bio).HasMaxLength(500);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithOne()
                  .HasForeignKey<Patient>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.BloodType).HasMaxLength(10);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.EmergencyContactName).HasMaxLength(100);
            entity.Property(e => e.EmergencyContactPhone).HasMaxLength(20);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<DoctorAvailability>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Doctor)
                  .WithMany()
                  .HasForeignKey(e => e.DoctorId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.DayOfWeek).IsRequired();
            entity.Property(e => e.SlotDurationMinutes).IsRequired();
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Patient)
                  .WithMany()
                  .HasForeignKey(e => e.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Doctor)
                  .WithMany()
                  .HasForeignKey(e => e.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(500);
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Patient)
                  .WithMany()
                  .HasForeignKey(e => e.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Doctor)
                  .WithMany()
                  .HasForeignKey(e => e.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Appointment)
                  .WithMany()
                  .HasForeignKey(e => e.AppointmentId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.Diagnosis).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Symptoms).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Patient)
                  .WithMany()
                  .HasForeignKey(e => e.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Doctor)
                  .WithMany()
                  .HasForeignKey(e => e.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.MedicalRecord)
                  .WithMany()
                  .HasForeignKey(e => e.MedicalRecordId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.MedicationName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Dosage).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Frequency).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Instructions).HasMaxLength(500);
        });
    }
}