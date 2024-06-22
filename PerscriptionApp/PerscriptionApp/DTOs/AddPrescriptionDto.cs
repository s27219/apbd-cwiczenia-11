namespace PerscriptionApp.DTOs;

public class AddPrescriptionDto
{
    public PatientDto Patient { get; set; } = null!;
    public int IdDoctor { get; set; }
    public List<MedicamentDto> Medicaments { get; set; } = null!;
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}

public class PatientDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Details { get; set; } = String.Empty;
}
