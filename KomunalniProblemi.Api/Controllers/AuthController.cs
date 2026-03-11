using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using KomunalniProblemi.Api.Dtos;
using KomunalniProblemi.Api.Helpers;
using KomunalniProblemi.Domain.Entities;
using KomunalniProblemi.Domain.Enums;
using KomunalniProblemi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KomunalniProblemi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // POST: /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Ime) || req.Ime.Trim().Length < 2) return BadRequest("Ime je obavezno.");
        if (string.IsNullOrWhiteSpace(req.Prezime) || req.Prezime.Trim().Length < 2) return BadRequest("Prezime je obavezno.");
        if (string.IsNullOrWhiteSpace(req.Email)) return BadRequest("Email je obavezan.");
        if (string.IsNullOrWhiteSpace(req.Password) || req.Password.Length < 6) return BadRequest("Lozinka mora imati bar 6 karaktera.");

        var email = req.Email.Trim().ToLowerInvariant();
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return BadRequest("Email nije validan.");

        var exists = await _db.Korisnici.AnyAsync(x => x.Email.ToLower() == email);
        if (exists) return BadRequest("Email je već zauzet.");

        PasswordHelper.CreatePasswordHash(req.Password, out var hash, out var salt);

        var korisnik = new Korisnik
        {
            Ime = req.Ime.Trim(),
            Prezime = req.Prezime.Trim(),
            Email = email,

            // byte[] varijanta
            PasswordHash = hash,
            PasswordSalt = salt,

            Uloga = Uloga.Gradjanin, // svi novi su gradjanin
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Korisnici.Add(korisnik);
        await _db.SaveChangesAsync();

        return Ok(new { korisnik.KorisnikID, korisnik.Email, uloga = korisnik.Uloga.ToString() });
    }

    // POST: /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Email i lozinka su obavezni.");

        var email = req.Email.Trim().ToLowerInvariant();

        var korisnik = await _db.Korisnici.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
        if (korisnik is null)
            return Unauthorized("Pogrešan email ili lozinka.");

        var hash = korisnik.PasswordHash;
        var salt = korisnik.PasswordSalt;

        if (hash is null || hash.Length == 0 || salt is null || salt.Length == 0)
            return Unauthorized("Pogrešan email ili lozinka.");

        var ok = PasswordHelper.VerifyPassword(req.Password, hash, salt);
        if (!ok)
            return Unauthorized("Pogrešan email ili lozinka.");

        // ===== JWT =====
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, korisnik.KorisnikID.ToString()),
            new Claim(ClaimTypes.Email, korisnik.Email),
            new Claim(ClaimTypes.Role, korisnik.Uloga.ToString())
        };

        var jwtKey = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(jwtKey))
            return StatusCode(500, "JWT Key nije podešen u appsettings.json");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenString,
            korisnik.KorisnikID,
            korisnik.Email,
            uloga = korisnik.Uloga.ToString()
        });
    }
}