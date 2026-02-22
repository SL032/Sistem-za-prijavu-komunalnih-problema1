namespace KomunalniProblemi.Api.Dtos;

public class UpdatePrijavaRequest
{
    public int? ProblemID { get; set; }
    public int? LokacijaID { get; set; }
    public int? SluzbaID { get; set; }
    public string? Opis { get; set; }
}