using Domain.Models;
using WebUI.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WebUI.Server.Services.AuthService;

public sealed class AuthService(IDbContextFactory<EcommerceContext> contextFactory,
    IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly IDbContextFactory<EcommerceContext> _contextFactory = contextFactory;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<ApplicationUser> userManager = userManager;
    
    public async Task<string> GetUserIdAsync()
    {
        ApplicationUser user = await userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User)
            ?? throw new Exception("User not found.");

        return user.Id;
    }

    public string? GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    }

    public async Task<ApplicationUser?> GetUserByEmail(string email)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email!.Equals(email));
    }
}
