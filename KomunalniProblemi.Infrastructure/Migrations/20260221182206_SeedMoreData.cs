using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KomunalniProblemi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Korisnici",
                columns: new[] { "KorisnikID", "Email", "Ime", "Prezime" },
                values: new object[] { 1, "petar@test.com", "Petar", "Petrović" });

            migrationBuilder.InsertData(
                table: "Lokacije",
                columns: new[] { "LokacijaID", "Adresa", "Opis" },
                values: new object[] { 1, "Kragujevac, Centar", "Kod pošte" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "KorisnikID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lokacije",
                keyColumn: "LokacijaID",
                keyValue: 1);
        }
    }
}
