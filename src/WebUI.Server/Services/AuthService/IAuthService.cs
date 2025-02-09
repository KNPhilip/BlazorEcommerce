using Domain.Models;

namespace WebUI.Server.Services.AuthService;

public interface IAuthService
{
    Task<string> GetUserIdAsync();
    string? GetUserEmail();
    Task<ApplicationUser?> GetUserByEmail(string email);
}
