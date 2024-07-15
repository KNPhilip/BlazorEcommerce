using WebUI.Server.Services.CartService;
using WebUI.Server.Services.CategoryService;
using WebUI.Server.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ASP.NET Configuration
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

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

// Entity Framework Configuration
builder.Services.AddDbContextFactory<EcommerceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration["TokenKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped(sp => new HttpClient 
{
    BaseAddress = new Uri(builder.Configuration["BaseUri"]!) 
});

// Add services to the container
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

#region Security Headers
// Referrer Policy Header - Controls included information on navigation
app.UseReferrerPolicy(options => options.SameOrigin());
// X Content Type Options Header - Prevents MIME-sniffing of the content type
// app.UseXContentTypeOptions();
// X Frame Options Header - Defends against attacks like clickjacking by banning framing on the site
app.UseXfo(options => options.Deny());
// X-Xss Protection Header (Old) - Protection from XSS attacks by analyzing the page and blocking seemingly malicious stuff
app.UseXXssProtection(options => options.EnabledWithBlockMode());
// Content Security Policy Header - Whitelists certain content and prevents other malicious assets (new XSS Protection)
app.UseCspReportOnly(options => options
    .BlockAllMixedContent()
    .StyleSources(s => s.Self().UnsafeInline().CustomSources("https://fonts.googleapis.com"))
    .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com"))
    .FormActions(s => s.Self())
    // Frame Ancestors makes X-Frame-Options obsolete
    .FrameAncestors(s => s.Self())
    .ImageSources(s => s.Self().CustomSources("blob:", "https://upload.wikimedia.org", "https://en.wikipedia.org", "data:"))
    .ScriptSources(s => s.Self().CustomSources("https://localhost:55150").UnsafeEval())
);
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // HTTP Strict Transport Security Header:
    // Strengthens implementation of TLS by enforcing the use of HTTPS
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

app.UseAntiforgery();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WebUI.Server.Client.Components._Imports).Assembly);

app.Run();