namespace BlazorEcommerce.Server.Services.AuthService
{
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
    }
}
