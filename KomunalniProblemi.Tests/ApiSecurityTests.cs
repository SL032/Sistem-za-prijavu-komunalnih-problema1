using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace KomunalniProblemi.Tests;

public class ApiSecurityTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiSecurityTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Prijave_GetAll_WithoutToken_Returns401()
    {
        var resp = await _client.GetAsync("/api/prijave");
        resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Prijave_GetAll_WithInvalidToken_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "NOT_A_TOKEN");

        var resp = await _client.GetAsync("/api/prijave");
        resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        _client.DefaultRequestHeaders.Authorization = null; 
    }

    [Fact]
    public async Task Auth_Register_Then_Login_ReturnsToken()
    {
        var token = await TestAuth.RegisterAndLoginAsync(_client, "u1@test.com", "Test123!");
        token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Prijave_Create_WithToken_Returns201()
    {
        var token = await TestAuth.RegisterAndLoginAsync(_client, "u2@test.com", "Test123!");

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var create = new
        {
            opis = "Rupa ispred zgrade",
            problemId = 1,
            lokacijaId = 1,
            sluzbaId = 1
        };

        var resp = await _client.PostAsJsonAsync("/api/prijave", create);

        resp.StatusCode.Should().Be(HttpStatusCode.Created);

        _client.DefaultRequestHeaders.Authorization = null; 
    }

    [Fact]
    public async Task Auth_Register_DuplicateEmail_Returns400()
    {
        var reg = new
        {
            ime = "Test",
            prezime = "User",
            email = "dup@test.com",
            password = "Test123!"
        };

        var r1 = await _client.PostAsJsonAsync("/api/auth/register", reg);
        r1.StatusCode.Should().Be(HttpStatusCode.OK);

        var r2 = await _client.PostAsJsonAsync("/api/auth/register", reg);
        r2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Auth_Login_WrongPassword_Returns401()
    {
        // napravi nalog
        var reg = new
        {
            ime = "Test",
            prezime = "User",
            email = "wrongpw@test.com",
            password = "Test123!"
        };

        var r1 = await _client.PostAsJsonAsync("/api/auth/register", reg);
        r1.StatusCode.Should().Be(HttpStatusCode.OK);

        // pogresna lozinka
        var login = new
        {
            email = "wrongpw@test.com",
            password = "NEISPRAVNO123!"
        };

        var r2 = await _client.PostAsJsonAsync("/api/auth/login", login);
        r2.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}