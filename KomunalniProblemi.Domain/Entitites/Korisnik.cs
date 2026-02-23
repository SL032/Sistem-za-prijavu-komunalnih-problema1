using System.ComponentModel.DataAnnotations;
using KomunalniProblemi.Domain.Enums;

namespace KomunalniProblemi.Domain.Entities;

public class Korisnik
{
    [Key]
    public int KorisnikID { get; set; }

    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Auth
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }

    public Uloga Uloga { get; set; } = Uloga.Gradjanin;

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Prijava> Prijave { get; set; } = new List<Prijava>();
}