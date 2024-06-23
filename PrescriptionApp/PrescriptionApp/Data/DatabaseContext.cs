using Microsoft.EntityFrameworkCore;
using PrescriptionApp.Models;

namespace PrescriptionApp.Data;

public class DatabaseContext : DbContext
{
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    public DbSet<User> Users { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
            {
                new Doctor {
                    IdDoctor = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@gmail.com"
                },
                new Doctor {
                    IdDoctor = 2,
                    FirstName = "Anna",
                    LastName = "Nowak",
                    Email = "anna.nowak@gmail.com"
                }
            });

            modelBuilder.Entity<Patient>().HasData(new List<Patient>
            {
                new Patient {
                    IdPatient = 1,
                    FirstName = "Adam",
                    LastName = "Nowak",
                    Birthdate = DateTime.Parse("2001-11-01"),
                },
                new Patient {
                    IdPatient = 2,
                    FirstName = "Aleksandra",
                    LastName = "Wiśniewska",
                    Birthdate = DateTime.Parse("2004-05-02"),
                }
            });

            modelBuilder.Entity<Medicament>().HasData(new List<Medicament>
            {
                new Medicament
                {
                    IdMedicament = 1,
                    Name = "Adderall",
                    Description = "Description_1",
                    Type = "Type_1"
                },
                new Medicament
                {
                    IdMedicament = 2,
                    Name = "Viagra",
                    Description = "Description_2",
                    Type = "Type_2"
                },
                new Medicament
                {
                    IdMedicament = 3,
                    Name = "Botox",
                    Description = "Description_3",
                    Type = "Type_3"
                }
            });

            modelBuilder.Entity<Prescription>().HasData(new List<Prescription>
            {
                new Prescription
                {
                    IdPrescription = 1,
                    Date = DateTime.Parse("2024-05-28"),
                    DueDate = DateTime.Parse("2024-05-29"),
                    IdPatient = 1,
                    IdDoctor = 2
                },
                new Prescription
                {
                    IdPrescription = 2,
                    Date = DateTime.Parse("2024-09-28"),
                    DueDate = DateTime.Parse("2024-11-29"),
                    IdPatient = 2,
                    IdDoctor = 1
                },
                new Prescription
                {
                    IdPrescription = 3,
                    Date = DateTime.Parse("2022-05-28"),
                    DueDate = DateTime.Parse("2025-08-03"),
                    IdPatient = 1,
                    IdDoctor = 1
                }
            });

            modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>
            {
                new PrescriptionMedicament
                {
                    IdMedicament = 1,
                    IdPrescription= 1,
                    Dose = 3,
                    Details = "Details_1"
                },
                new PrescriptionMedicament
                {
                    IdMedicament = 1,
                    IdPrescription= 3,
                    Dose = 4,
                    Details = "Details_2"
                },
                new PrescriptionMedicament
                {
                    IdMedicament = 2,
                    IdPrescription= 2,
                    Dose = 2,
                    Details = "Details_3"
                },
                new PrescriptionMedicament
                {
                    IdMedicament = 2,
                    IdPrescription= 1,
                    Dose = 12,
                    Details = "Details_4"
                }
            });
    }
}