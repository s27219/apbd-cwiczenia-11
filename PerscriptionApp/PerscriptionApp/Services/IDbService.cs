using PerscriptionApp.DTOs;
using PerscriptionApp.Models;

namespace PerscriptionApp.Services;

public interface IDbService
{
    Task<bool> DoesPatientExist(int idPatient);
    Task<bool> DoesDoctorExist(int idDoctor);
    Task<bool> DoesMedicamentExist(int idMedicament);
    Task AddNewPatient(Patient patient);
    Task AddNewPrescription(Prescription prescription);
    Task AddPrescriptionMedicaments(IEnumerable<PrescriptionMedicament> prescriptionMedicaments);
    public Task<GetPatientDto> GetPatientDetails(int idPatient);
}