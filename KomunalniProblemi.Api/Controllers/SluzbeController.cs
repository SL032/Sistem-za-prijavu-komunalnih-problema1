using KomunalniProblemi.Api.Dtos;
using KomunalniProblemi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KomunalniProblemi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SluzbeController : ControllerBase
{
    private readonly AppDbContext _db;

    public SluzbeController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<KomunalnaSluzbaDto>>> GetAll()
    {
        var result = await _db.KomunalneSluzbe
            .AsNoTracking()
            .OrderBy(x => x.SluzbaID)
            .Select(x => new KomunalnaSluzbaDto(
                x.SluzbaID,
                x.Naziv,
                x.Kontakt
            ))
            .ToListAsync();

        return Ok(result);
    }
}