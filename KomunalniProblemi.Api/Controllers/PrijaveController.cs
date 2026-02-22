using KomunalniProblemi.Api.Dtos;
using KomunalniProblemi.Domain.Enums;
using KomunalniProblemi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KomunalniProblemi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrijaveController : ControllerBase
{
    private readonly AppDbContext _db;

    public PrijaveController(AppDbContext db) => _db = db;

    // GET: /api/prijave?status&problemId&sluzbaId&from&to&search&page&pageSize
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? status,
        [FromQuery] int? problemId,
        [FromQuery] int? sluzbaId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var q = _db.Prijave
            .AsNoTracking()
            .Include(x => x.KomunalniProblem)
            .Include(x => x.Lokacija)
            .Include(x => x.KomunalnaSluzba)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<StatusPrijave>(status, ignoreCase: true, out var st))
            q = q.Where(x => x.Status == st);

        if (problemId.HasValue)
            q = q.Where(x => x.ProblemID == problemId.Value);

        if (sluzbaId.HasValue)
            q = q.Where(x => x.SluzbaID == sluzbaId.Value);

        if (from.HasValue)
            q = q.Where(x => x.Datum >= from.Value);

        if (to.HasValue)
            q = q.Where(x => x.Datum <= to.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            q = q.Where(x =>
                x.Opis.Contains(s) ||
                x.Lokacija.Adresa.Contains(s) ||
                x.KomunalniProblem.Naziv.Contains(s));
        }

        var totalCount = await q.CountAsync();

        var items = await q
            .OrderByDescending(x => x.Datum)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new PrijavaListItemDto(
                x.PrijavaID,
                x.Datum,
                x.Opis,
                x.Status.ToString(),
                x.KorisnikID,
                x.ProblemID,
                x.KomunalniProblem.Naziv,
                x.LokacijaID,
                x.Lokacija.Adresa,
                x.SluzbaID,
                x.KomunalnaSluzba != null ? x.KomunalnaSluzba.Naziv : null
            ))
            .ToListAsync();

        return Ok(new
        {
            value = items,
            count = totalCount,
            page,
            pageSize
        });
    }

    // GET: /api/prijave/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _db.Prijave
            .AsNoTracking()
            .Include(x => x.KomunalniProblem)
            .Include(x => x.Lokacija)
            .Include(x => x.KomunalnaSluzba)
            .Where(x => x.PrijavaID == id)
            .Select(x => new PrijavaListItemDto(
                x.PrijavaID,
                x.Datum,
                x.Opis,
                x.Status.ToString(),
                x.KorisnikID,
                x.ProblemID,
                x.KomunalniProblem.Naziv,
                x.LokacijaID,
                x.Lokacija.Adresa,
                x.SluzbaID,
                x.KomunalnaSluzba != null ? x.KomunalnaSluzba.Naziv : null
            ))
            .FirstOrDefaultAsync();

        return item is null ? NotFound() : Ok(item);
    }

    // POST: /api/prijave
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePrijavaRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Opis) || req.Opis.Trim().Length < 5)
            return BadRequest("Opis je obavezan i mora imati bar 5 karaktera.");

        if (!await _db.Korisnici.AnyAsync(x => x.KorisnikID == req.KorisnikID))
            return BadRequest("Ne postoji korisnik.");

        if (!await _db.KomunalniProblemi.AnyAsync(x => x.ProblemID == req.ProblemID))
            return BadRequest("Ne postoji izabrani komunalni problem.");

        if (!await _db.Lokacije.AnyAsync(x => x.LokacijaID == req.LokacijaID))
            return BadRequest("Ne postoji izabrana lokacija.");

        if (req.SluzbaID.HasValue &&
            !await _db.KomunalneSluzbe.AnyAsync(x => x.SluzbaID == req.SluzbaID.Value))
            return BadRequest("Ne postoji izabrana komunalna služba.");

        var prijava = new KomunalniProblemi.Domain.Entities.Prijava
        {
            Datum = DateTime.UtcNow,
            Opis = req.Opis.Trim(),
            Status = StatusPrijave.Novo,
            KorisnikID = req.KorisnikID,
            ProblemID = req.ProblemID,
            LokacijaID = req.LokacijaID,
            SluzbaID = req.SluzbaID
        };

        _db.Prijave.Add(prijava);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = prijava.PrijavaID }, new { prijava.PrijavaID });
    }

    // PUT: /api/prijave/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePrijavaRequest req)
    {
        var prijava = await _db.Prijave.FirstOrDefaultAsync(x => x.PrijavaID == id);
        if (prijava is null) return NotFound();

        if (prijava.Status != StatusPrijave.Novo)
            return BadRequest("Izmena je dozvoljena samo dok je status 'Novo'.");

        if (req.Opis is not null)
        {
            var opis = req.Opis.Trim();
            if (opis.Length < 5) return BadRequest("Opis mora imati bar 5 karaktera.");
            prijava.Opis = opis;
        }

        if (req.ProblemID.HasValue)
        {
            var exists = await _db.KomunalniProblemi.AnyAsync(x => x.ProblemID == req.ProblemID.Value);
            if (!exists) return BadRequest("Ne postoji izabrani komunalni problem.");
            prijava.ProblemID = req.ProblemID.Value;
        }

        if (req.LokacijaID.HasValue)
        {
            var exists = await _db.Lokacije.AnyAsync(x => x.LokacijaID == req.LokacijaID.Value);
            if (!exists) return BadRequest("Ne postoji izabrana lokacija.");
            prijava.LokacijaID = req.LokacijaID.Value;
        }

        if (req.SluzbaID.HasValue)
        {
            var exists = await _db.KomunalneSluzbe.AnyAsync(x => x.SluzbaID == req.SluzbaID.Value);
            if (!exists) return BadRequest("Ne postoji izabrana komunalna služba.");
            prijava.SluzbaID = req.SluzbaID.Value;
        }

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // PATCH: /api/prijave/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest req)
    {
        var prijava = await _db.Prijave.FirstOrDefaultAsync(x => x.PrijavaID == id);
        if (prijava is null) return NotFound();

        if (!Enum.TryParse<StatusPrijave>(req.Status, ignoreCase: true, out var newStatus))
            return BadRequest("Neispravan status. Dozvoljeno: Novo, UObradi, Reseno, Odbijeno.");

        var oldStatus = prijava.Status;

        var allowed =
            (oldStatus == StatusPrijave.Novo && newStatus == StatusPrijave.UObradi) ||
            (oldStatus == StatusPrijave.UObradi && (newStatus == StatusPrijave.Reseno || newStatus == StatusPrijave.Odbijeno));

        if (!allowed)
            return BadRequest($"Nedozvoljena promena statusa: {oldStatus} -> {newStatus}");

        prijava.Status = newStatus;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: /api/prijave/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var prijava = await _db.Prijave.FirstOrDefaultAsync(x => x.PrijavaID == id);
        if (prijava is null) return NotFound();

        if (prijava.Status != StatusPrijave.Novo)
            return BadRequest("Brisanje je dozvoljeno samo dok je status 'Novo'.");

        _db.Prijave.Remove(prijava);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}