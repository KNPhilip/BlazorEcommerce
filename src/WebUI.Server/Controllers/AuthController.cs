using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebUI.Server.Controllers;

public sealed class AuthController(
    IAuthService authService) : ControllerTemplate
{
    private readonly IAuthService _authService = authService;

    //[HttpPost("register")]
    //public async Task<ActionResult<int>> Register(UserRegisterDto request) =>
    //    HandleResult(await _authService.Register(new ApplicationUser
    //    { Email = request.Email }, request.Password));

    //[HttpPost("login")]
    //public async Task<ActionResult<string>> Login(UserLoginDto request) =>
    //    HandleResult(await _authService.Login(request.Email, request.Password));

    //[HttpPost("change-password"), Authorize]
    //public async Task<ActionResult<bool>> ChangePassword([FromBody] string newPassword) =>
    //    HandleResult(await _authService.ChangePassword(int.Parse(User
    //        .FindFirstValue(ClaimTypes.NameIdentifier)!), newPassword));

    //[HttpPost("create-password-token")]
    //public async Task<ActionResult<string>> CreateResetToken(ApplicationUser request) =>
    //    HandleResult(await _authService.CreateResetToken(request));

    //[HttpPost("reset-password")]
    //public async Task<ActionResult<bool>> ResetPassword(PasswordResetDto request) =>
    //    HandleResult(await _authService.ResetPassword(request.UserEmail!, request.NewPassword, request.ResetToken!));

    //[HttpPost("reset-password/validate")]
    //public async Task<ActionResult<bool>> ResetPasswordTokenValidation(TokenValidateDto request) =>
    //    HandleResult(await _authService.ValidateResetPasswordToken(request.UserEmail!, request.ResetToken!)); 
}
