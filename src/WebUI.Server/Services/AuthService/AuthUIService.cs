﻿using Domain.Dtos;
using Domain.Interfaces;
using Domain.Models;
using System.Security.Claims;

namespace WebUI.Server.Services.AuthService;

public sealed class AuthUIService(
    IHttpContextAccessor httpContextAccessor,
    IAuthService authService) : IAuthUIService
{
    public async Task<bool> IsUserAuthenticated()
    {
        await Task.Delay(0);
        bool? isAuthenticated = httpContextAccessor
            .HttpContext!.User.Identity!.IsAuthenticated;
        if (isAuthenticated is null)
        {
            return false;
        }
        return isAuthenticated.Value;
    }

    public async Task<bool> ChangePassword(UserChangePasswordDto request)
    {
        //ResponseDto<bool> result = await authService.ChangePassword(
        //    int.Parse(httpContextAccessor.HttpContext!.User
        //    .FindFirstValue(ClaimTypes.NameIdentifier)!), 
        //    request.NewPassword);
        //return result.Data;
        return true;
    }

    public async Task<string?> CreateResetToken(ApplicationUser request)
    {
        //ResponseDto<string> result = await authService
        //    .CreateResetToken(request);
        //return result.Data;
        return "";
    }

    public async Task<string?> Login(UserLoginDto request)
    {
        //ResponseDto<string> result = await authService
        //    .Login(request.Email, request.Password);
        //return result.Data;
        return "";
    }

    public async Task<bool> Register(UserRegisterDto request)
    {
        //ResponseDto<int> result = await authService.Register(new ApplicationUser
        //    { Email = request.Email }, request.Password);
        //return result.Success;
        return true;
    }

    public async Task<bool> ResetPassword(PasswordResetDto request)
    {
        //ResponseDto<bool> result = await authService.ResetPassword(
        //    request.UserEmail!, request.NewPassword, request.ResetToken!);
        //return result.Data;
        return true;
    }

    public async Task<bool> ValidateResetPasswordToken(TokenValidateDto request)
    {
        //ResponseDto<bool> result = await authService
        //    .ValidateResetPasswordToken(request.UserEmail!, request.ResetToken!);
        //return result.Data;
        return true;
    }
}
