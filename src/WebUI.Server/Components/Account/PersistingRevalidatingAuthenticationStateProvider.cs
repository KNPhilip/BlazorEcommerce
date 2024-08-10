using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;

namespace WebUI.Server.Components.Account;

internal sealed class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly PersistentComponentState state;
    private readonly IdentityOptions options;

    private readonly PersistingComponentStateSubscription subscription;

    private Task<AuthenticationState>? authenticationStateTask;

    public PersistingRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory serviceScopeFactory,
        PersistentComponentState persistentComponentState,
        IOptions<IdentityOptions> optionsAccessor)
        : base(loggerFactory)
    {
        scopeFactory = serviceScopeFactory;
        state = persistentComponentState;
        options = optionsAccessor.Value;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return await ValidateSecurityStampAsync(userManager, authenticationState.User);
    }

    private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        ApplicationUser? user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return false;
        }
        else if (!userManager.SupportsUserSecurityStamp)
        {
            return true;
        }
        else
        {
            string? principalStamp = principal.FindFirstValue(options.ClaimsIdentity.SecurityStampClaimType);
            string userStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp == userStamp;
        }
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        authenticationStateTask = task;
    }

    private async Task OnPersistingAsync()
    {
        if (authenticationStateTask is null)
        {
            throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
        }

        AuthenticationState authenticationState = await authenticationStateTask;
        ClaimsPrincipal principal = authenticationState.User;

        if (principal.Identity?.IsAuthenticated is true)
        {
            string? userId = principal.FindFirst(options.ClaimsIdentity.UserIdClaimType)?.Value;
            string? email = principal.FindFirst(options.ClaimsIdentity.EmailClaimType)?.Value;

            if (userId is not null && email is not null)
            {
                state.PersistAsJson(nameof(UserInfoDto), new UserInfoDto()
                {
                    UserId = userId,
                    Email = email,
                    Roles = []
                });
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        subscription.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        base.Dispose(disposing);
    }
}
