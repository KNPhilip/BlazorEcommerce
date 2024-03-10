using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Server.Client.Services.AuthService
{
    public class AuthUIService : IAuthUIService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthUIService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> ChangePassword(UserChangePasswordDto request)
        {
            var result = await _http.PostAsJsonAsync("api/v1/auth/change-password", request.NewPassword);
            return await result.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> IsUserAuthenticated() =>
            (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;

        public async Task<string?> Login(UserLoginDto request)
        {
            var result = await _http.PostAsJsonAsync("api/v1/auth/login", request);
            return await result.Content.ReadFromJsonAsync<string>();
        }

        public async Task<bool> Register(UserRegisterDto request)
        {
            var result = await _http.PostAsJsonAsync("api/v1/auth/register", request);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> ValidateResetPasswordToken(TokenValidateDto request)
        {
            var result = await _http.PostAsJsonAsync("api/v1/auth/reset-password/validate", request);
            return await result.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> ResetPassword(PasswordResetDto request)
        {
            var result = await _http.PostAsJsonAsync("api/v1/auth/reset-password", request);
            return await result.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<string?> CreateResetToken(User request)
        {
            var result = await _http.PostAsJsonAsync("api/v1/Auth/create-password-token", request);
            return await result.Content.ReadFromJsonAsync<string?>();
        }
    }
}
