namespace BlazorEcommerce.Server.Controllers
{
    public class AuthController : ControllerTemplate
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request) =>
            HandleResult(await _authService.Register(new User 
                { Email = request.Email }, request.Password));

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request) =>
            HandleResult(await _authService.Login(request.Email, request.Password));

        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword) =>
            HandleResult(await _authService.ChangePassword(int.Parse(User
                .FindFirstValue(ClaimTypes.NameIdentifier)!), newPassword));

        [HttpPost("create-password-token")]
        public async Task<ActionResult<ServiceResponse<string>>> CreateResetToken(User request) =>
            HandleResult(await _authService.CreateResetToken(request));

        [HttpPost("reset-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPassword(PasswordResetDto request) =>
            HandleResult(await _authService.ResetPassword(request.UserEmail!, request.NewPassword, request.ResetToken!));

        [HttpPost("reset-password/validate")]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPasswordTokenValidation(TokenValidateDto request) =>
            HandleResult(await _authService.ValidateResetPasswordToken(request.UserEmail!, request.ResetToken!));
    }
}