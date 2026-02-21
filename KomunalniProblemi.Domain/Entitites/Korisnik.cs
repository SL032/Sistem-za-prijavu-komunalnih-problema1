using System.ComponentModel.DataAnnotations;

namespace KomunalniProblemi.Domain.Entities;

public class Korisnik
{
    [Key]
    public int KorisnikID { get; set; }

    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Prijava> Prijave { get; set; } = new List<Prijava>();
}