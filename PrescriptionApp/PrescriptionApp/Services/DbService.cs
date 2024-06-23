using Microsoft.EntityFrameworkCore;
using PrescriptionApp.Data;
using PrescriptionApp.DTOs;
using PrescriptionApp.Models;


namespace PrescriptionApp.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> DoesPatientExist(int idPatient)
    {
        return await _context.Patients.AnyAsync(e => e.IdPatient == idPatient);
    }

    public async Task<bool> DoesDoctorExist(int idDoctor)
    {
        return await _context.Doctors.AnyAsync(e => e.IdDoctor == idDoctor);
    }

    public async Task<bool> DoesMedicamentExist(int idMedicament)
    {
        return await _context.Patients.AnyAsync(e => e.IdPatient == idMedicament);
    }

    public async Task AddNewPatient(Patient patient)
    {
        await _context.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task AddNewPrescription(Prescription prescription)
    {
        await _context.AddAsync(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task AddPrescriptionMedicaments(IEnumerable<PrescriptionMedicament> prescriptionMedicaments)
    {
        await _context.AddRangeAsync(prescriptionMedicaments);
        await _context.SaveChangesAsync();
    }
    
    public async Task<GetPatientDto> GetPatientDetails(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Where(p => p.IdPatient == idPatient)
            .FirstOrDefaultAsync();

        if (patient == null)
        {
            return null;
        }

        return new GetPatientDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(pr => pr.DueDate)
                .Select(pr => new GetPrescriptionDto
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new GetDoctorDto
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName
                    },
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new GetMedicamentDto
                    {
                        IdMedicament = pm.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Medicament.Description
                    }).ToList()
                }).ToList()
        };
    }
    
    public async Task<bool> UserExists(string login)
    {
        return await _context.Users.AnyAsync(u => u.Login == login);
    }

    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<User> GetUserByLogin(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    /*public async Task ChangeRefreshToken(User user, string newToken, DateTime newTokenExp)
    {
        user.RefreshToken = newToken;
        user.RefreshTokenExp = newTokenExp;
        _context.SaveChangesAsync();
    }*/
    
    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<User> GetUserByToken(string token)
    {
        return await _context.Users.Where(u => u.RefreshToken == token).FirstOrDefaultAsync();
    }


}