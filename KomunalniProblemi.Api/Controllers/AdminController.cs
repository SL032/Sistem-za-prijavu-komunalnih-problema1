using KomunalniProblemi.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KomunalniProblemi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _db.Korisnici
            .Select(x => new
            {
                x.KorisnikID,
                x.Email,
                Uloga = x.Uloga.ToString()
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpPatch("set-role/{id:int}")]
    public async Task<IActionResult> SetRole(int id, [FromBody] string role)
    {
        var user = await _db.Korisnici.FindAsync(id);
        if (user is null)
            return NotFound();

        if (!Enum.TryParse<KomunalniProblemi.Domain.Enums.Uloga>(role, true, out var newRole))
            return BadRequest("Neispravna uloga.");

        user.Uloga = newRole;
        await _db.SaveChangesAsync();

        return NoContent();
    }
}