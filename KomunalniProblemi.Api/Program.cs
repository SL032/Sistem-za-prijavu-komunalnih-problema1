using System.Text;
using KomunalniProblemi.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Controllers (za /api/... rute)
builder.Services.AddControllers();

// EF Core (LocalDB / SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Auth
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Missing Jwt:Key in appsettings.json");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // ok za dev
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
            ValidIssuer = jwtIssuer,

            ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// OpenAPI endpoint (development)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Auth middleware (MORA pre MapControllers)
app.UseAuthentication();
app.UseAuthorization();

// Health check
app.MapGet("/health", () => Results.Ok("OK"))
   .WithName("Health");

// Map controllers
app.MapControllers();

app.Run();