using System.ComponentModel.DataAnnotations;

namespace KomunalniProblemi.Domain.Entities;

public class KomunalnaSluzba
{
    [Key]
    public int SluzbaID { get; set; }

    public string Naziv { get; set; } = string.Empty;
    public string? Kontakt { get; set; }

    public ICollection<Prijava> Prijave { get; set; } = new List<Prijava>();
}