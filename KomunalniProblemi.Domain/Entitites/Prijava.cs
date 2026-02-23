using System.ComponentModel.DataAnnotations;
using KomunalniProblemi.Domain.Enums;

namespace KomunalniProblemi.Domain.Entities;

public class Prijava
{
    [Key]
    public int PrijavaID { get; set; }

    public DateTime Datum { get; set; } = DateTime.UtcNow;
    public string Opis { get; set; } = string.Empty;
    public StatusPrijave Status { get; set; } = StatusPrijave.Novo;

    public int KorisnikID { get; set; }
    public Korisnik Korisnik { get; set; } = null!;

    public int ProblemID { get; set; }
    public KomunalniProblem KomunalniProblem { get; set; } = null!;

    public int LokacijaID { get; set; }
    public Lokacija Lokacija { get; set; } = null!;

    public int? SluzbaID { get; set; }
    public KomunalnaSluzba? KomunalnaSluzba { get; set; }

    public ICollection<Dokument> Dokumenti { get; set; } = new List<Dokument>();
}