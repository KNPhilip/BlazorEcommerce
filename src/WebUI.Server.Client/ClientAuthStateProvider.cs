using Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace WebUI.Server.Client;

internal sealed class ClientAuthStateProvider : AuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> defaultUnauthenticatedTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private readonly Task<AuthenticationState> authenticationStateTask = defaultUnauthenticatedTask;

    public ClientAuthStateProvider(PersistentComponentState state)
    {
        if (!state.TryTakeFromJson<UserInfoDto>(nameof(UserInfoDto), out var userInfo) || userInfo is null)
        {
            return;
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email) ];

        authenticationStateTask = Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                authenticationType: nameof(ClientAuthStateProvider)))));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => authenticationStateTask;
}
