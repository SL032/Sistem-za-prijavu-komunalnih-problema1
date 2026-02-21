using System.ComponentModel.DataAnnotations;

namespace KomunalniProblemi.Domain.Entities;

public class KomunalniProblem
{
    [Key]
    public int ProblemID { get; set; }

    public string Naziv { get; set; } = string.Empty;
    public string? Opis { get; set; }

    public ICollection<Prijava> Prijave { get; set; } = new List<Prijava>();
}