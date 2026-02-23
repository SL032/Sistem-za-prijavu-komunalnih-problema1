namespace KomunalniProblemi.Api.Dtos;

public record PrijavaListItemDto(
    int PrijavaID,
    DateTime Datum,
    string Opis,
    string Status,
    int KorisnikID,
    int ProblemID,
    string ProblemNaziv,
    int LokacijaID,
    string LokacijaAdresa,
    int? SluzbaID,
    string? SluzbaNaziv
);