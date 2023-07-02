namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await _authService.Register(
                new User 
                { 
                    Email = request.Email
                }, 
                request.Password
            );

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authService.Login(request.Email, request.Password);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _authService.ChangePassword(int.Parse(userId), newPassword);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("create-password-token")]
        public async Task<ActionResult<ServiceResponse<string>>> CreateResetToken(User request)
        {
            var response = await _authService.CreateResetToken(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpPost("reset-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPassword(PasswordResetDto request)
        {
            var response = await _authService.ResetPassword(request.UserEmail, request.NewPassword, request.ResetToken);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}