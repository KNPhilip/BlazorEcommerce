using Domain.Interfaces;
using WebUI.Server.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

namespace WebUI.Server.Client.Extensions;

public static class Extensions
{
    public static void AddServicesFromExtensionsClass
        (this IServiceCollection services, Uri baseAddress)
    {
        services.AddMudServices();
        services.AddBlazoredLocalStorage();
        services.AddCascadingAuthenticationState();

        services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });
        services.AddScoped<AuthenticationStateProvider, ClientAuthStateProvider>();
        services.AddScoped<IAddressUIService, AddressUIService>();
        services.AddScoped<IAuthUIService, AuthUIService>();
        services.AddScoped<ICartUIService, CartUIService>();
        services.AddScoped<ICategoryUIService, CategoryUIService>();
        services.AddScoped<IOrderUIService, OrderUIService>();
        services.AddScoped<IProductUIService, ProductUIService>();
        services.AddScoped<IProductTypeUIService, ProductTypeUIService>();

        services.AddOptions();
        services.AddAuthorizationCore();
    }
}
