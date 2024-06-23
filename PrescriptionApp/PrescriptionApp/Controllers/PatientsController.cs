using Microsoft.AspNetCore.Mvc;
using PrescriptionApp.DTOs;
using PrescriptionApp.Services;

namespace PrescriptionApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IDbService _dbService;
    public PatientsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    
    [HttpGet("{idPatient}")]
    public async Task<ActionResult<GetPatientDto>> GetPatient(int idPatient)
    {
        if (!await _dbService.DoesPatientExist(idPatient))
            return NotFound($"Patient with given ID - {idPatient} doesn't exist");
        
        var patientDetails = await _dbService.GetPatientDetails(idPatient);
        

        return Ok(patientDetails);
    }
}