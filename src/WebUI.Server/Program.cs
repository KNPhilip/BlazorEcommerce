using WebUI.Server.Services.CartService;
using WebUI.Server.Services.CategoryService;
using WebUI.Server.Components;
using WebUI.Server.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using WebUI.Server.Data;
using Blazored.LocalStorage;
using WebUI.Server.Services.AddressService;
using WebUI.Server.Services.MailService;
using WebUI.Server.Services.OrderService;
using WebUI.Server.Services.PaymentService;
using WebUI.Server.Services.ProductService;
using WebUI.Server.Services.ProductTypeService;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Domain.Dtos;
using MudBlazor.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Domain.Models;
using WebUI.Server.Components.Account;
using System.Reflection;

Assembly assembly = typeof(Program).Assembly;
__packageId = assembly.GetCustomAttribute<AssemblyPackageIdAttribute>()?.PackageId ?? "WebUI.Server";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ASP.NET Configuration
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authorization"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EcommerceContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<EcommerceContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped(sp => new HttpClient 
{
    BaseAddress = new Uri(builder.Configuration["BaseUri"]!) 
});

builder.Services.AddScoped<IAddressUIService, AddressUIService>();
builder.Services.AddScoped<IAuthUIService, AuthUIService>();
builder.Services.AddScoped<ICartUIService, CartUIService>();
builder.Services.AddScoped<ICategoryUIService, CategoryUIService>();
builder.Services.AddScoped<IOrderUIService, OrderUIService>();
builder.Services.AddScoped<IProductUIService, ProductUIService>();
builder.Services.AddScoped<IProductTypeUIService, ProductTypeUIService>();

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();

builder.Services.AddOptions<MailSettingsDto>().Bind(builder.Configuration
    .GetSection(MailSettingsDto.SectionName));

WebApplication app = builder.Build();

app.UseReferrerPolicy(options => options.SameOrigin());
app.UseXfo(options => options.Deny());
app.UseXXssProtection(options => options.EnabledWithBlockMode());
app.UseCspReportOnly(options => options
    .BlockAllMixedContent()
    .StyleSources(s => s.Self().UnsafeInline().CustomSources("https://fonts.googleapis.com"))
    .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com"))
    .FormActions(s => s.Self())
    .FrameAncestors(s => s.Self())
    .ImageSources(s => s.Self().CustomSources("blob:", "https://upload.wikimedia.org", "https://en.wikipedia.org", "data:"))
    .ScriptSources(s => s.Self().CustomSources("https://localhost:55150").UnsafeEval())
);

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.Use(async (context, next) =>
    {
        #pragma warning disable ASP0019 // Suggest using IHeaderDictionary.Append or the indexer
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        #pragma warning restore ASP0019 // Suggest using IHeaderDictionary.Append or the indexer
        await next.Invoke();
    });
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WebUI.Client.Components._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();

app.Run();

public sealed partial class Program
{
    private static string? __packageId;

    public static string PackageId
    {
        get
        {
            lock (__packageId!)
            {
                return __packageId;
            }
        }
    }
}
