using System.ComponentModel.DataAnnotations;

namespace KomunalniProblemi.Domain.Entities;

public class Lokacija
{
    [Key]
    public int LokacijaID { get; set; }

    public string Adresa { get; set; } = string.Empty;
    public string? Opis { get; set; }

    public ICollection<Prijava> Prijave { get; set; } = new List<Prijava>();
}