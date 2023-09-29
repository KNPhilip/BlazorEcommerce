#region Usings
global using Microsoft.AspNetCore.Mvc;
global using BlazorEcommerce.Shared;
global using Microsoft.EntityFrameworkCore;
global using BlazorEcommerce.Server.Data;
global using BlazorEcommerce.Server.Services.ProductService;
global using BlazorEcommerce.Server.Services.CategoryService;
global using BlazorEcommerce.Server.Services.CartService;
global using BlazorEcommerce.Server.Services.AuthService;
global using BlazorEcommerce.Server.Services.OrderService;
global using BlazorEcommerce.Server.Services.PaymentService;
global using BlazorEcommerce.Server.Services.AddressService;
global using BlazorEcommerce.Server.Services.ProductTypeService;
global using BlazorEcommerce.Server.Services.MailService;
global using BlazorEcommerce.Shared.Models;
global using BlazorEcommerce.Shared.Dtos;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using Stripe.Checkout;
global using Microsoft.AspNetCore.Authorization;
global using System.Net.Mail;
global using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
#endregion

var builder = WebApplication.CreateBuilder(args);

// Entity Framework Configuration
builder.Services.AddDbContext<EcommerceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Add Services
// Add Services to IoC container
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddSingleton(builder.Configuration.GetSection("MailSettings").Get<MailSettingsDto>());
builder.Services.AddHttpContextAccessor(); 
#endregion
// Add Authentication Middleware
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

var app = builder.Build();

#region Security Headers
// Referrer Policy Header - Controls included information on navigation
app.UseReferrerPolicy(options => options.SameOrigin());
// X Content Type Options Header - Prevents MIME-sniffing of the content type
app.UseXContentTypeOptions();
// X Frame Options Header - Defends against attacks like clickjacking by banning framing on the site
app.UseXfo(options => options.Deny());
// X-Xss Protection Header (Old) - Protection from XSS attacks by analyzing the page and blocking seemingly malicious stuff
app.UseXXssProtection(options => options.EnabledWithBlockMode());
// Content Security Policy Header - Whitelists certain content and prevents other malicious assets (new XSS Protection)
app.UseCspReportOnly(options => options
    .BlockAllMixedContent()
    .StyleSources(s => s.Self())
    .FontSources(s => s.Self())
    .FormActions(s => s.Self())
    // Frame Ancestors makes X-Frame-Options obsolete
    .FrameAncestors(s => s.Self())
    .ImageSources(s => s.Self())
    .ScriptSources(s => s.Self())
); 
#endregion

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // HTTP Strict Transport Security Header:
    // Strengthens implementation of TLS by enforcing the use of HTTPS
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        await next.Invoke();
    });
}

app.UseSwagger();
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();