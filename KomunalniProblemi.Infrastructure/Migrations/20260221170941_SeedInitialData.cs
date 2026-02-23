using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KomunalniProblemi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "KomunalneSluzbe",
                columns: new[] { "SluzbaID", "Kontakt", "Naziv" },
                values: new object[,]
                {
                    { 1, "inspekcija@komunalno.cacak.rs", "Komunalna inspekcija" },
                    { 2, "rasveta@komunalno.cacak.rs", "Javna rasveta" }
                });

            migrationBuilder.InsertData(
                table: "KomunalniProblemi",
                columns: new[] { "ProblemID", "Naziv", "Opis" },
                values: new object[,]
                {
                    { 1, "Rupa na putu", "Oštećenje puta" },
                    { 2, "Javna rasveta", "Neispravna/ugašena rasveta" },
                    { 3, "Otpad", "Divlja deponija / neodneto smeće" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KomunalneSluzbe",
                keyColumn: "SluzbaID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KomunalneSluzbe",
                keyColumn: "SluzbaID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "KomunalniProblemi",
                keyColumn: "ProblemID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KomunalniProblemi",
                keyColumn: "ProblemID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "KomunalniProblemi",
                keyColumn: "ProblemID",
                keyValue: 3);
        }
    }
}
