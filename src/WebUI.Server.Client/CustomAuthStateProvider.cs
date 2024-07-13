using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorEcommerce.Server.Client;

public sealed class CustomAuthStateProvider(ILocalStorageService localStorage,
    HttpClient http) : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage = localStorage;
    private readonly HttpClient _http = http;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? authToken = await _localStorage.GetItemAsStringAsync("authToken");

        ClaimsIdentity identity = new();
        _http.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(authToken))
        {
            try
            {
                IEnumerable<Claim> claims = ParseClaimsFromJwt(authToken);
                identity = new ClaimsIdentity(claims, "jwt");
                if (ExpiredToken(identity))
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
                else throw new Exception();
            }
            catch (Exception)
            {
                await _localStorage.RemoveItemAsync("authToken");
                identity = new ClaimsIdentity();
            }
        }

        ClaimsPrincipal user = new(identity);
        AuthenticationState state = new(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private static bool ExpiredToken(ClaimsIdentity? identity)
    {
        if (identity is null)
            return true;

        ClaimsPrincipal user = new(identity);
        Claim exp = user.FindFirst("exp")!;

        if (exp is null)
            return true;

        DateTimeOffset expDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp.Value));
        if (DateTime.Now > expDateTime)
            return true;

        return false;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }
        return Convert.FromBase64String(base64);
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);

        Dictionary<string, object> keyValuePairs = JsonSerializer
            .Deserialize<Dictionary<string, object>>(jsonBytes)!;

        IEnumerable<Claim> claims = keyValuePairs
            .Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));

        return claims;
    }
}
