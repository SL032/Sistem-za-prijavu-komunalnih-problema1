namespace KomunalniProblemi.Api.Dtos;

public class RegisterRequest
{
    public string Ime { get; set; } = "";
    public string Prezime { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}