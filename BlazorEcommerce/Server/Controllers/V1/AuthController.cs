namespace BlazorEcommerce.Server.Controllers.V1
{
    /// <summary>
    /// Auth Controller - Contains all endpoints regarding token-based authentication and authorization.
    /// </summary>
    public class AuthController : ControllerTemplate
    {
        /// <summary>
        /// IAuthService field. Used to access the Auth Services.
        /// </summary>
        private readonly IAuthService _authService;

        /// <param name="authService">IAuthService instance to be passed on to the
        /// field, containing the correct implementation class through the IoC container.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint to register a new user.
        /// </summary>
        /// <param name="request">Represents the given register
        /// DTO containing info about the to-be registered user.</param>
        /// <returns>The ID of the new registered user or an error in case of failure.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request) =>
            HandleResult(await _authService.Register(new User
            { Email = request.Email }, request.Password));

        /// <summary>
        /// Endpoint for login.
        /// </summary>
        /// <param name="request">Represents the given login DTO,
        /// containing the information to log the user in.</param>
        /// <returns>A new valid JWT for future authentication and authorization.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request) =>
            HandleResult(await _authService.Login(request.Email, request.Password));

        /// <summary>
        /// Endpoint for changing the authenticated users current password.
        /// </summary>
        /// <param name="newPassword">Represents the given new password to change to.</param>
        /// <returns>True/False depending on the success.</returns>
        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword) =>
            HandleResult(await _authService.ChangePassword(int.Parse(User
                .FindFirstValue(ClaimTypes.NameIdentifier)!), newPassword));

        /// <summary>
        /// Endpoint for the user to request a password reset.
        /// </summary>
        /// <param name="request">Represents the given User to create a
        /// Password Reset Token for.</param>
        /// <returns>A string with instructions for the user on what to do next.</returns>
        [HttpPost("create-password-token")]
        public async Task<ActionResult<ServiceResponse<string>>> CreateResetToken(User request) =>
            HandleResult(await _authService.CreateResetToken(request));

        /// <summary>
        /// Endpoint for resetting the authenticated users password if they have the valid Password Reset Token.
        /// </summary>
        /// <param name="request">Represents the given info about the user to reset the password for.</param>
        /// <returns>True/False depending on the success.</returns>
        [HttpPost("reset-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPassword(PasswordResetDto request) =>
            HandleResult(await _authService.ResetPassword(request.UserEmail!, request.NewPassword, request.ResetToken!));

        /// <summary>
        /// Endpoint for validating the given Password Reset Token.
        /// </summary>
        /// <param name="request">Represents the given info to validate the token based on.</param>
        /// <returns>True/False depending on the success.</returns>
        [HttpPost("reset-password/validate")]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPasswordTokenValidation(TokenValidateDto request) =>
            HandleResult(await _authService.ValidateResetPasswordToken(request.UserEmail!, request.ResetToken!));
    }
}