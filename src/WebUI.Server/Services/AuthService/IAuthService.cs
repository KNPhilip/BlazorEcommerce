using Domain.Dtos;
using Domain.Models;

namespace WebUI.Server.Services.AuthService;

public interface IAuthService
{
    //Task<ResponseDto<int>> Register(ApplicationUser user, string password);
    //Task<ResponseDto<string>> Login(string email, string password);
    //Task<ResponseDto<bool>> ChangePassword(int userId, string newPassword);
    //Task<ResponseDto<string>> CreateResetToken(ApplicationUser request);
    //Task<ResponseDto<bool>> ValidateResetPasswordToken(string email, string resetToken);
    //Task<ResponseDto<bool>> ResetPassword(string email, string newPassword, string resetToken);
    int GetNameIdFromClaims();
    string? GetUserEmail();
    Task<ApplicationUser?> GetUserByEmail(string email);
}
