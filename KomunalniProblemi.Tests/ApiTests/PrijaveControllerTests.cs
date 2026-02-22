using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using KomunalniProblemi.Api.Controllers;
using KomunalniProblemi.Api.Dtos;
using KomunalniProblemi.Domain.Entities;
using KomunalniProblemi.Domain.Enums;
using KomunalniProblemi.Infrastructure.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KomunalniProblemi.Tests.ApiTests;

public class PrijaveControllerTests
{
    private static AppDbContext NewDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }

    private static async Task SeedAsync(AppDbContext db)
    {
        db.Korisnici.Add(new Korisnik { KorisnikID = 1, Ime = "Test", Prezime = "User", Email = "test@test.com" });
        db.Lokacije.Add(new Lokacija { LokacijaID = 1, Adresa = "Kragujevac, Centar", Opis = "Test" });
        db.KomunalniProblemi.Add(new KomunalniProblem { ProblemID = 1, Naziv = "Rupa na putu", Opis = "Test" });
        db.KomunalneSluzbe.Add(new KomunalnaSluzba { SluzbaID = 1, Naziv = "Komunalna inspekcija", Kontakt = "x@y.com" });

        db.Prijave.Add(new Prijava
        {
            PrijavaID = 1,
            Datum = DateTime.UtcNow,
            Opis = "Rupa ispred zgrade",
            Status = StatusPrijave.Novo,
            KorisnikID = 1,
            ProblemID = 1,
            LokacijaID = 1,
            SluzbaID = 1
        });

        await db.SaveChangesAsync();
    }

    private static IEnumerable<PrijavaListItemDto> ExtractValueFromPagedOk(OkObjectResult ok)
    {
        
        var payload = ok.Value!;
        var valueProp = payload.GetType().GetProperty("value")!;
        return (IEnumerable<PrijavaListItemDto>)valueProp.GetValue(payload)!;
    }

    [Fact]
    public async Task GetAll_ReturnsOk_AndItems()
    {
        var db = NewDb(nameof(GetAll_ReturnsOk_AndItems));
        await SeedAsync(db);

        var controller = new PrijaveController(db);

        
        var result = await controller.GetAll(
            status: null,
            problemId: null,
            sluzbaId: null,
            from: null,
            to: null,
            search: null,
            page: 1,
            pageSize: 10
        );

        var ok = Assert.IsType<OkObjectResult>(result);
        var items = ExtractValueFromPagedOk(ok);

        Assert.Single(items);
    }

    [Fact]
    public async Task Delete_WhenStatusNotNovo_ReturnsBadRequest()
    {
        var db = NewDb(nameof(Delete_WhenStatusNotNovo_ReturnsBadRequest));
        await SeedAsync(db);

        var p = await db.Prijave.FirstAsync();
        p.Status = StatusPrijave.UObradi;
        await db.SaveChangesAsync();

        var controller = new PrijaveController(db);

        var result = await controller.Delete(1);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Novo", bad.Value?.ToString() ?? "");
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        var db = NewDb(nameof(GetById_WhenNotFound_ReturnsNotFound));
        await SeedAsync(db);

        var controller = new PrijaveController(db);

        var result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateStatus_WhenTransitionNotAllowed_ReturnsBadRequest()
    {
        var db = NewDb(nameof(UpdateStatus_WhenTransitionNotAllowed_ReturnsBadRequest));
        await SeedAsync(db);

        var controller = new PrijaveController(db);

        var req = new UpdateStatusRequest { Status = "Reseno" };
        var result = await controller.UpdateStatus(1, req);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Nedozvoljena", bad.Value?.ToString() ?? "");
    }
}