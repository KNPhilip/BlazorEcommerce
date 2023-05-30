namespace BlazorEcommerce.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDto request);
        Task<ServiceResponse<string>> Login(UserLoginDto request);
    }
}