﻿namespace BlazorEcommerce.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegisterDto request);
        Task<string?> Login(UserLoginDto request);
        Task<bool> ChangePassword(UserChangePasswordDto request);
        Task<bool> ResetPassword(PasswordResetDto request);
        Task<bool> ValidateResetPasswordToken(TokenValidateDto request);
        Task<string?> CreateResetToken(User request);
        Task<bool> IsUserAuthenticated();
    }
}