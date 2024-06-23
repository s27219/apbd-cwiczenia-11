using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrescriptionApp.DTOs;
using PrescriptionApp.Models;
using PrescriptionApp.Services;

namespace PrescriptionApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService _dbService;
    public PrescriptionsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionDto addPrescriptionDto)
    {
        Patient newPatient = null;
        Prescription newPrescription;
        if (!await _dbService.DoesPatientExist(addPrescriptionDto.Patient.IdPatient))
        {
            newPatient = new Patient
            {
                FirstName = addPrescriptionDto.Patient.FirstName,
                LastName = addPrescriptionDto.Patient.LastName,
                Birthdate = addPrescriptionDto.Patient.Birthdate
            };
            newPrescription = new Prescription
            {
                Date = addPrescriptionDto.Date,
                DueDate = addPrescriptionDto.DueDate,
                IdDoctor = addPrescriptionDto.IdDoctor,
                Patient = newPatient
            };
        }
        else
        {
            newPrescription = new Prescription
            {
                Date = addPrescriptionDto.Date,
                DueDate = addPrescriptionDto.DueDate,
                IdPatient = addPrescriptionDto.Patient.IdPatient,
                IdDoctor = addPrescriptionDto.IdDoctor,
            };
        }
        if (!await _dbService.DoesDoctorExist(addPrescriptionDto.IdDoctor))
            return NotFound($"Doctor with given ID - {addPrescriptionDto.IdDoctor} doesn't exist");

        if (addPrescriptionDto.Medicaments.Count > 10)
            return BadRequest("Prescription can only hold up to 10 Medicaments");
        
        
        
        var medicaments = new List<PrescriptionMedicament>();
        foreach (var med in  addPrescriptionDto.Medicaments)
        {
            if (!await _dbService.DoesMedicamentExist(med.IdMedicament))
                return NotFound($"Medicament with given ID - {med.IdMedicament} doesn't exist");
            medicaments.Add(new PrescriptionMedicament
            {
                IdMedicament = med.IdMedicament,
                Dose = med.Dose,
                Details = med.Details,
                Prescription = newPrescription
            });
        }

        if (!(addPrescriptionDto.DueDate >= addPrescriptionDto.Date))
            return BadRequest("DateDue can not be earlier than Date");
        
        try
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (newPatient != null)
                    await _dbService.AddNewPatient(newPatient);
                await _dbService.AddNewPrescription(newPrescription);
                await _dbService.AddPrescriptionMedicaments(medicaments);

                scope.Complete();
            }

            return Created("api/orders", new
            {
                newPrescription.IdPrescription,
                newPrescription.Date,
                newPrescription.DueDate,
                newPrescription.IdPatient,
                newPrescription.IdDoctor
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    
}