namespace BlazorEcommerce.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> UserExists(string email);
    }
}