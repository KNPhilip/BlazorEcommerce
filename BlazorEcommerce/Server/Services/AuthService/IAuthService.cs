namespace BlazorEcommerce.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<bool> UserExists(string email);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
        Task<ServiceResponse<string>> CreateResetToken(User request);
        Task<ServiceResponse<bool>> ValidateResetPasswordToken(string email, string resetToken);
        Task<ServiceResponse<bool>> ResetPassword(string email, string newPassword, string resetToken);
        int GetNameIdFromClaims();
        string GetUserEmail();
        Task<User> GetUserByEmail(string email);
    }
}