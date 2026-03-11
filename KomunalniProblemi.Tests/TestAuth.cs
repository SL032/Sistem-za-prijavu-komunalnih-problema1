using System.Net.Http.Json;

namespace KomunalniProblemi.Tests;
//helper za reg i login + uzimanje tokena
public static class TestAuth
{
    public static async Task<string> RegisterAndLoginAsync(HttpClient client, string email, string password)
    {
        var reg = new
        {
            ime = "Test",
            prezime = "User",
            email,
            password
        };

        var regResp = await client.PostAsJsonAsync("/api/auth/register", reg);
        regResp.EnsureSuccessStatusCode();

        var login = new { email, password };
        var loginResp = await client.PostAsJsonAsync("/api/auth/login", login);
        loginResp.EnsureSuccessStatusCode();

        var obj = await loginResp.Content.ReadFromJsonAsync<LoginResponse>();
        return obj!.token;
    }

    private sealed class LoginResponse
    {
        public string token { get; set; } = "";
    }
}