﻿using Domain.Dtos;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace WebUI.Client.Services;

public sealed class AuthUIService(
    AuthenticationStateProvider authStateProvider,
    HttpClient http) : IAuthUIService
{
    public async Task<bool> ChangePassword(UserChangePasswordDto request)
    {
        HttpResponseMessage result = await http
            .PostAsJsonAsync("api/v1/auth/change-password", request.NewPassword);
        return await result.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<bool> IsUserAuthenticated()
    {
        return (await authStateProvider
            .GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
    }

    public async Task<string?> Login(UserLoginDto request)
    {
        HttpResponseMessage result = await http
            .PostAsJsonAsync("api/v1/auth/login", request);
        return await result.Content
            .ReadFromJsonAsync<string>();
    }

    public async Task<bool> Register(UserRegisterDto request)
    {
        HttpResponseMessage result = await http
            .PostAsJsonAsync("api/v1/auth/register", request);
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> ValidateResetPasswordToken(TokenValidateDto request)
    {
        HttpResponseMessage result = await http
            .PostAsJsonAsync("api/v1/auth/reset-password/validate", request);
        return await result.Content
            .ReadFromJsonAsync<bool>();
    }

    public async Task<bool> ResetPassword(PasswordResetDto request)
    {
        HttpResponseMessage result = await http
            .PostAsJsonAsync("api/v1/auth/reset-password", request);
        return await result.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<string?> CreateResetToken(ApplicationUser request)
    {
        HttpResponseMessage result = await http
            .PostAsJsonAsync("api/v1/Auth/create-password-token", request);
        return await result.Content.ReadFromJsonAsync<string?>();
    }
}
