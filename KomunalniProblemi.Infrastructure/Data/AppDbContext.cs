using Microsoft.EntityFrameworkCore;
using KomunalniProblemi.Domain.Entities;

namespace KomunalniProblemi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Korisnik> Korisnici => Set<Korisnik>();
    public DbSet<Prijava> Prijave => Set<Prijava>();
    public DbSet<Dokument> Dokumenti => Set<Dokument>();
    public DbSet<Lokacija> Lokacije => Set<Lokacija>();
    public DbSet<KomunalniProblem> KomunalniProblemi => Set<KomunalniProblem>();
    public DbSet<KomunalnaSluzba> KomunalneSluzbe => Set<KomunalnaSluzba>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Prijava>()
            .HasOne(p => p.Korisnik)
            .WithMany(k => k.Prijave)
            .HasForeignKey(p => p.KorisnikID);

        modelBuilder.Entity<Prijava>()
            .HasOne(p => p.KomunalniProblem)
            .WithMany(kp => kp.Prijave)
            .HasForeignKey(p => p.ProblemID);

        modelBuilder.Entity<Prijava>()
            .HasOne(p => p.Lokacija)
            .WithMany(l => l.Prijave)
            .HasForeignKey(p => p.LokacijaID);

        modelBuilder.Entity<Prijava>()
            .HasOne(p => p.KomunalnaSluzba)
            .WithMany(s => s.Prijave)
            .HasForeignKey(p => p.SluzbaID);

        modelBuilder.Entity<Dokument>()
            .HasOne(d => d.Prijava)
            .WithMany(p => p.Dokumenti)
            .HasForeignKey(d => d.PrijavaID);

        modelBuilder.Entity<KomunalniProblem>().HasData(
            new KomunalniProblem { ProblemID = 1, Naziv = "Rupa na putu", Opis = "Oštećenje puta" },
            new KomunalniProblem { ProblemID = 2, Naziv = "Javna rasveta", Opis = "Neispravna/ugašena rasveta" },
            new KomunalniProblem { ProblemID = 3, Naziv = "Otpad", Opis = "Divlja deponija / neodneto smeće" }
        );

        modelBuilder.Entity<KomunalnaSluzba>().HasData(
            new KomunalnaSluzba { SluzbaID = 1, Naziv = "Komunalna inspekcija", Kontakt = "inspekcija@komunalno.cacak.rs" },
            new KomunalnaSluzba { SluzbaID = 2, Naziv = "Javna rasveta", Kontakt = "rasveta@komunalno.cacak.rs" }
        );
    }
}