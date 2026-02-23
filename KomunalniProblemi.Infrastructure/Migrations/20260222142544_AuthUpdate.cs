using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KomunalniProblemi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Korisnici",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Korisnici",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Korisnici",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Korisnici",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Uloga",
                table: "Korisnici",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Gradjanin");

            migrationBuilder.UpdateData(
                table: "Korisnici",
                keyColumn: "KorisnikID",
                keyValue: 1,
                columns: new[] { "CreatedAtUtc", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_Email",
                table: "Korisnici",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Korisnici_Email",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "Uloga",
                table: "Korisnici");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
