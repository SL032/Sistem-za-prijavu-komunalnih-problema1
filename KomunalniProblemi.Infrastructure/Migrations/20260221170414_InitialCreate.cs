using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KomunalniProblemi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KomunalneSluzbe",
                columns: table => new
                {
                    SluzbaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kontakt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KomunalneSluzbe", x => x.SluzbaID);
                });

            migrationBuilder.CreateTable(
                name: "KomunalniProblemi",
                columns: table => new
                {
                    ProblemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KomunalniProblemi", x => x.ProblemID);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    KorisnikID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.KorisnikID);
                });

            migrationBuilder.CreateTable(
                name: "Lokacije",
                columns: table => new
                {
                    LokacijaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lokacije", x => x.LokacijaID);
                });

            migrationBuilder.CreateTable(
                name: "Prijave",
                columns: table => new
                {
                    PrijavaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    KorisnikID = table.Column<int>(type: "int", nullable: false),
                    ProblemID = table.Column<int>(type: "int", nullable: false),
                    LokacijaID = table.Column<int>(type: "int", nullable: false),
                    SluzbaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijave", x => x.PrijavaID);
                    table.ForeignKey(
                        name: "FK_Prijave_KomunalneSluzbe_SluzbaID",
                        column: x => x.SluzbaID,
                        principalTable: "KomunalneSluzbe",
                        principalColumn: "SluzbaID");
                    table.ForeignKey(
                        name: "FK_Prijave_KomunalniProblemi_ProblemID",
                        column: x => x.ProblemID,
                        principalTable: "KomunalniProblemi",
                        principalColumn: "ProblemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prijave_Korisnici_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prijave_Lokacije_LokacijaID",
                        column: x => x.LokacijaID,
                        principalTable: "Lokacije",
                        principalColumn: "LokacijaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dokumenti",
                columns: table => new
                {
                    DokumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrijavaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dokumenti", x => x.DokumentID);
                    table.ForeignKey(
                        name: "FK_Dokumenti_Prijave_PrijavaID",
                        column: x => x.PrijavaID,
                        principalTable: "Prijave",
                        principalColumn: "PrijavaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dokumenti_PrijavaID",
                table: "Dokumenti",
                column: "PrijavaID");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_KorisnikID",
                table: "Prijave",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_LokacijaID",
                table: "Prijave",
                column: "LokacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_ProblemID",
                table: "Prijave",
                column: "ProblemID");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_SluzbaID",
                table: "Prijave",
                column: "SluzbaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dokumenti");

            migrationBuilder.DropTable(
                name: "Prijave");

            migrationBuilder.DropTable(
                name: "KomunalneSluzbe");

            migrationBuilder.DropTable(
                name: "KomunalniProblemi");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Lokacije");
        }
    }
}
