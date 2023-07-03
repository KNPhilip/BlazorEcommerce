namespace BlazorEcommerce.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDto request);
        Task<ServiceResponse<string>> Login(UserLoginDto request);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePasswordDto request);
        Task<ServiceResponse<bool>> ResetPassword(PasswordResetDto request);
        Task<ServiceResponse<bool>> ValidateResetPasswordToken(TokenValidateDto request);
        Task<ServiceResponse<string>> CreateResetToken(User request);
        Task<bool> IsUserAuthenticated();
    }
}