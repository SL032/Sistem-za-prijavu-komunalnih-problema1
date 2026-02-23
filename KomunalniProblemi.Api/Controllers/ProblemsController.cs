using KomunalniProblemi.Api.Dtos;
using KomunalniProblemi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KomunalniProblemi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProblemsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProblemsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<KomunalniProblemDto>>> GetAll()
    {
        var result = await _db.KomunalniProblemi
            .AsNoTracking()
            .OrderBy(x => x.ProblemID)
            .Select(x => new KomunalniProblemDto(
                x.ProblemID,
                x.Naziv,
                x.Opis
            ))
            .ToListAsync();

        return Ok(result);
    }
}