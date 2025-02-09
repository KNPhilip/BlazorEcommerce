using Domain.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebUI.Client.Services;

public sealed class AuthUIService(
    AuthenticationStateProvider authStateProvider) : IAuthUIService
{
    public async Task<bool> IsUserAuthenticated()
    {
        return (await authStateProvider
            .GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
    }
}
