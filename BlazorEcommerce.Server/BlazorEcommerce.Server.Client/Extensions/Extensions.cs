using BlazorEcommerce.Server.Client.Services.AddressService;
using BlazorEcommerce.Server.Client.Services.AuthService;
using BlazorEcommerce.Server.Client.Services.CartService;
using BlazorEcommerce.Server.Client.Services.CategoryService;
using BlazorEcommerce.Server.Client.Services.OrderService;
using BlazorEcommerce.Server.Client.Services.ProductService;
using BlazorEcommerce.Server.Client.Services.ProductTypeService;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorEcommerce.Server.Client.Extensions
{
    public static class Extensions
    {
        public static void AddServicesFromExtensionsClass
            (this IServiceCollection services, Uri baseAddress)
        {
            // services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();

            services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            services.AddScoped<IAddressUIService, AddressUIService>();
            services.AddScoped<IAuthUIService, AuthUIService>();
            services.AddScoped<ICartUIService, CartUIService>();
            services.AddScoped<ICategoryUIService, CategoryUIService>();
            services.AddScoped<IOrderUIService, OrderUIService>();
            services.AddScoped<IProductUIService, ProductUIService>();
            services.AddScoped<IProductTypeUIService, ProductTypeUIService>();

            services.AddBlazoredLocalStorage();
        }
    }
}
