using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Interfaces;
using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Services.AuthService;

public sealed class AuthUIService(
    IHttpContextAccessor httpContextAccessor) : IAuthUIService
{
    public bool IsUserAuthenticated()
    {
        bool? isAuthenticated = httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
        if (isAuthenticated is null)
        {
            return false;
        }
        return isAuthenticated.Value;
    }

    public Task<bool> ChangePassword(UserChangePasswordDto request) =>
        throw new NotImplementedException();

    public Task<string?> CreateResetToken(User request) =>
        throw new NotImplementedException();

    public Task<string?> Login(UserLoginDto request) =>
        throw new NotImplementedException();

    public Task<bool> Register(UserRegisterDto request) =>
        throw new NotImplementedException();

    public Task<bool> ResetPassword(PasswordResetDto request) =>
        throw new NotImplementedException();

    public Task<bool> ValidateResetPasswordToken(TokenValidateDto request) =>
        throw new NotImplementedException();

    Task<bool> IAuthUIService.IsUserAuthenticated() =>
        throw new NotImplementedException();
}
