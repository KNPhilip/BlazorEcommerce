using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Data;
using WebUI.Server.Services.MailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebUI.Server.Services.AuthService;

public sealed class AuthService(
    EcommerceContext context,
    IConfiguration configuration,
    IHttpContextAccessor httpContextAccessor,
    IMailService mailService) : IAuthService
{
    private readonly EcommerceContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IMailService _mailService = mailService;

    public async Task<ResponseDto<string>> Login(string email, string password)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));

        if (user is null)
        {
            return ResponseDto<string>.ErrorResponse("User not found.");
        }
        else if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return ResponseDto<string>.ErrorResponse("Incorrect password");
        }

        return ResponseDto<string>.SuccessResponse(CreateJWT(user));
    }

    public async Task<ResponseDto<int>> Register(User user, string password)
    {
        if (await UserExists(user.Email))
        {
            return ResponseDto<int>.ErrorResponse("User already exists.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return ResponseDto<int>.SuccessResponse(user.Id);
    }

    public async Task<ResponseDto<string>> CreateResetToken(User request)
    {
        User? user = await GetUserByEmail(request.Email);

        if (user is null)
        {
            return ResponseDto<string>.ErrorResponse(
                $"There are no registered users with the email address {request.Email}");
        }

        if (!String.IsNullOrEmpty(user.PasswordResetToken) && DateTime.Now < user.ResetTokenExpires)
        {
            return ResponseDto<string>.ErrorResponse(
                "There is an open reset request already! Please check your inbox or try again later");
        }

        user.PasswordResetToken = CreateRandomToken();
        user.ResetTokenExpires = DateTime.Now.AddHours(2);
        await _context.SaveChangesAsync();

        SendMailDto mail = new()
        {
            ToEmail = user.Email,
            Subject = "Password Reset",
            HTMLBody = "<h5>Hi there,</h5>you've requested to reset your password. Please use the following link to reset your password: <a href=\"https://localhost:7010/reset-password/?token=" + user.PasswordResetToken + "&user=" + user.Email + "\">Reset Password</a><br><br>" +
                "If the provided link is not clickable to you, please follow the steps below:<br>" +
                    "<b>1.</b> mark this (https://localhost:7010/reset-password/?token=" + user.PasswordResetToken + "&user=" + user.Email + ") link and copy it to the clipboard<br>" +
                "<b>2. </b> Paste this link into your browsers url and hit enter to visit the page." +
                "<br><br><b><span style=\"color:red;\">If you've not initiated this password change request, please reach out to us ASAP.</b>"
        };

        await _mailService.SendEmailAsync(mail.ToEmail, mail.Subject, mail.HTMLBody);

        return ResponseDto<string>.SuccessResponse("Please check your inbox to reset the password.");
    }

    public async Task<ResponseDto<bool>> ResetPassword(string email, string newPassword, string resetToken)
    {
        ResponseDto<bool> response = await ValidateResetPasswordToken(email, resetToken);
        if (!response.Success)
        {
            return response;
        }

        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
        {
            return ResponseDto<bool>.ErrorResponse($"User with email {email} not found.");
        }

        if (user.ResetTokenExpires < DateTime.Now)
        {
            return ResponseDto<bool>.ErrorResponse("This reset token has expired..");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.PasswordResetToken = "";
        user.ResetTokenExpires = null;
        await _context.SaveChangesAsync();

        return response;
    }

    public async Task<ResponseDto<bool>> ValidateResetPasswordToken(string email, string resetToken)
    {
        if (!await _context.Users.AnyAsync(user => user.PasswordResetToken
            .ToLower().Equals(resetToken.ToLower())))
        {
            return ResponseDto<bool>.ErrorResponse("Invalid reset token");
        }

        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
        {
            return ResponseDto<bool>.ErrorResponse($"User with email {email} not found.");
        }
        else if (user.ResetTokenExpires < DateTime.Now)
        {
            return ResponseDto<bool>.ErrorResponse("This reset token has expired..");
        }

        return ResponseDto<bool>.SuccessResponse(true);
    }

    public async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<ResponseDto<bool>> ChangePassword(int userId, string newPassword)
    {
        User? user = await _context.Users.FindAsync(userId);

        if (user is null)
        {
            return ResponseDto<bool>.ErrorResponse("User not found.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

        await _context.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }

    public int GetNameIdFromClaims()
    {
        return int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public string? GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
    }

    private static string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

    private string CreateJWT(User user)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["TokenKey"]!));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new(
            claims: claims,
            signingCredentials: creds,
            expires: DateTime.Now.AddDays(2)
        );

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    } 
}
