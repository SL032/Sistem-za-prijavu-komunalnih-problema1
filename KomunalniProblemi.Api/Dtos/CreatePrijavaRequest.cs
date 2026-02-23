namespace KomunalniProblemi.Api.Dtos;

public class CreatePrijavaRequest
{
    public int KorisnikID { get; set; }
    public int ProblemID { get; set; }
    public int LokacijaID { get; set; }
    public int? SluzbaID { get; set; }
    public string Opis { get; set; } = string.Empty;
}