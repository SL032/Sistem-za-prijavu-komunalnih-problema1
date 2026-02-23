namespace KomunalniProblemi.Api.Dtos;

public record KomunalnaSluzbaDto(
    int SluzbaID,
    string Naziv,
    string? Kontakt
);