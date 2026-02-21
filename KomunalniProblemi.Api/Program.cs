using KomunalniProblemi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers (za /api/... rute)
builder.Services.AddControllers();

// EF Core (LocalDB / SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// OpenAPI 
builder.Services.AddOpenApi();

var app = builder.Build();

// OpenAPI endpoint (development)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Health check
app.MapGet("/health", () => Results.Ok("OK"))
   .WithName("Health");

// Map controllers
app.MapControllers();

app.Run();