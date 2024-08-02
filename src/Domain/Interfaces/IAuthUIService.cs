namespace Domain.Interfaces;

public interface IAuthUIService
{
    Task<bool> Register(UserRegisterDto request);
    Task<string?> Login(UserLoginDto request);
    Task<bool> ChangePassword(UserChangePasswordDto request);
    Task<bool> ResetPassword(PasswordResetDto request);
    Task<bool> ValidateResetPasswordToken(TokenValidateDto request);
    Task<string?> CreateResetToken(ApplicationUser request);
    Task<bool> IsUserAuthenticated();
}
