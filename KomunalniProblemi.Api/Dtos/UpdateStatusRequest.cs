namespace KomunalniProblemi.Api.Dtos;

public class UpdateStatusRequest
{
    public string Status { get; set; } = string.Empty; // "Novo", "UObradi", "Reseno", "Odbijeno"
}