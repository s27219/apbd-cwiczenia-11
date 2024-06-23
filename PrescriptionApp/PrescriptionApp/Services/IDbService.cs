

using PrescriptionApp.DTOs;
using PrescriptionApp.Models;

namespace PrescriptionApp.Services;

public interface IDbService
{
    Task<bool> DoesPatientExist(int idPatient);
    Task<bool> DoesDoctorExist(int idDoctor);
    Task<bool> DoesMedicamentExist(int idMedicament);
    Task AddNewPatient(Patient patient);
    Task AddNewPrescription(Prescription prescription);
    Task AddPrescriptionMedicaments(IEnumerable<PrescriptionMedicament> prescriptionMedicaments);
    public Task<GetPatientDto> GetPatientDetails(int idPatient);
    public Task<bool> UserExists(string login);
    public Task AddUser(User user);
    public Task<User> GetUserByLogin(string login);
    public Task UpdateUser(User user);
    public Task<User> GetUserByToken(string token);
}