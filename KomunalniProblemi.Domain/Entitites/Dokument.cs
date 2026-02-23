using System.ComponentModel.DataAnnotations;

namespace KomunalniProblemi.Domain.Entities;

public class Dokument
{
    [Key]
    public int DokumentID { get; set; }

    public string Naziv { get; set; } = string.Empty;
    public string Tip { get; set; } = string.Empty;

    public int PrijavaID { get; set; }
    public Prijava Prijava { get; set; } = null!;
}