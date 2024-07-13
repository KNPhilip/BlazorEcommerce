using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Services.AuthService;

public interface IAuthService
{
    Task<ResponseDto<int>> Register(User user, string password);
    Task<bool> UserExists(string email);
    Task<ResponseDto<string>> Login(string email, string password);
    Task<ResponseDto<bool>> ChangePassword(int userId, string newPassword);
    Task<ResponseDto<string>> CreateResetToken(User request);
    Task<ResponseDto<bool>> ValidateResetPasswordToken(string email, string resetToken);
    Task<ResponseDto<bool>> ResetPassword(string email, string newPassword, string resetToken);
    int GetNameIdFromClaims();
    string? GetUserEmail();
    Task<User?> GetUserByEmail(string email);
}
