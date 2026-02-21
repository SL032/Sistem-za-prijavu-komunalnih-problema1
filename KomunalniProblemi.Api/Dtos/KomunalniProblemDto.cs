namespace KomunalniProblemi.Api.Dtos;

public record KomunalniProblemDto(
    int ProblemID,
    string Naziv,
    string? Opis
);