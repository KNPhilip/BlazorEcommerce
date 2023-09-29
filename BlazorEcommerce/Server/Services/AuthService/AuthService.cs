using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Services.AuthService
{
    /// <summary>
    /// Implementation class of IAuthService.
    /// </summary>
    public class AuthService : IAuthService
    {
        #region Fields
        /// <summary>
        /// EcommerceContext field. Used to access the database context.
        /// </summary>
        private readonly EcommerceContext _context;
        /// <summary>
        /// IConfiguration instance - Represents a set of key/value application configuration properties.
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Instance of the HTTP Context.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// IMailService field. Used to access the Mail Services.
        /// </summary>
        private readonly IMailService _mailService;
        #endregion

        #region Constructor
        /// <param name="context">EcommerceContext instance to be passed on to the correct
        /// field, containing the correct implementation through the IoC container.</param>
        /// <param name="configuration">IConfiguration instance to be passed on to the correct
        /// correct field, containing the correct implementation through the IoC container.</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor instance to be passed on to the
        /// correct field, containing the correct implementation through the IoC container.</param>
        /// <param name="mailService">IMailService instance to be passed on to the correct
        /// field, containing the correct implementation class through the IoC container.</param>
        public AuthService(
            EcommerceContext context,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IMailService mailService)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Request to login to the application with a given email address and password.
        /// </summary>
        /// <param name="email">Represents the given email address to login to.</param>
        /// <param name="password">Represents the given password for the login.</param>
        /// <returns>A newly issued JWT for future authentication and authorization.</returns>
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            ServiceResponse<string> response = new();
            User? user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user is null)
                return new ServiceResponse<string> { Error = "User not found." };
            else if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return new ServiceResponse<string> { Error = "Incorrect password" };

            return ServiceResponse<string>.SuccessResponse(CreateJWT(user));
        }

        /// <summary>
        /// Registers a new User with the info of the given input.
        /// </summary>
        /// <param name="user">Represents the given user to register.</param>
        /// <param name="password">Represents the given password of the to-be registered user.</param>
        /// <returns>Returns the newly registered user's ID, or an appropriate error message in case of failure.</returns>
        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await UserExists(user.Email))
                return new ServiceResponse<int> { Error = "User already exists." };

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return ServiceResponse<int>.SuccessResponse(user.Id);
        }

        /// <summary>
        /// Creates a new password reset token for the user matching the email from the body.
        /// After that, sending the reset token to the users email address.
        /// </summary>
        /// <param name="request">Represents the given user to create a reset token for.</param>
        /// <returns>Instructions on what to do next, or an appropriate error message in case of failure.</returns>
        public async Task<ServiceResponse<string>> CreateResetToken(User request)
        {
            User? user = await GetUserByEmail(request.Email);
            if (user is null)
                return new ServiceResponse<string>
                {
                    Error =
                    $"There are no registered users with the email address {request.Email}"
                };

            if (!String.IsNullOrEmpty(user.PasswordResetToken) && DateTime.Now < user.ResetTokenExpires)
                return new ServiceResponse<string>
                {
                    Error =
                    "There is an open reset request already! Please check your inbox or try again later"
                };

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

            return ServiceResponse<string>.SuccessResponse("Please check your inbox to reset the password.");
        }

        /// <summary>
        /// Resets the password of the user from the database with the given email address,
        /// changing it to the new given password, if the given password reset token is valid.
        /// </summary>
        /// <param name="email">Represents the given email address to reset the password for.</param>
        /// <param name="newPassword">Represents the given new password.</param>
        /// <param name="resetToken">Represents the (hopyfully) valid Password Reset Token.</param>
        /// <returns>True/False depending on the response.</returns>
        public async Task<ServiceResponse<bool>> ResetPassword(string email, string newPassword, string resetToken)
        {
            ServiceResponse<bool> response = await ValidateResetPasswordToken(email, resetToken);
            if (!response.Success) return response;

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
                return new ServiceResponse<bool> { Error = $"User with email {email} not found." };
            else if (user.ResetTokenExpires < DateTime.Now)
                return new ServiceResponse<bool> { Error = "This reset token has expired.." };

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetToken = "";
            user.ResetTokenExpires = null;
            await _context.SaveChangesAsync();

            return response;
        }

        /// <summary>
        /// Validates the given password reset token for the given email address.
        /// </summary>
        /// <param name="email">Represents the given email address to validate the
        /// Password Reset Token for.</param>
        /// <param name="resetToken">Represents the (hopefully) valid Password Refresh Token.</param>
        /// <returns>True/False depending on the response,
        /// or an appropriate error message in case of failure.</returns>
        public async Task<ServiceResponse<bool>> ValidateResetPasswordToken(string email, string resetToken)
        {
            if (!await _context.Users.AnyAsync(user => user.PasswordResetToken.ToLower().Equals(resetToken.ToLower())))
                return new ServiceResponse<bool> { Error = "Invalid reset token" };

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                return new ServiceResponse<bool> { Error = $"User with email {email} not found." };
            else if (user.ResetTokenExpires < DateTime.Now)
                return new ServiceResponse<bool> { Error = "This reset token has expired.." };

            return ServiceResponse<bool>.SuccessResponse(true);
        }

        /// <summary>
        /// Checks if a user with the given email address is registered.
        /// </summary>
        /// <param name="email">Represents the given email address of the (hopefully) existing user.</param>
        /// <returns>True/False depending on the response.</returns>
        public async Task<bool> UserExists(string email) =>
            await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()));

        /// <summary>
        /// Changes the password of the user with the given ID to the new given password.
        /// </summary>
        /// <param name="userId">Represents the given user ID of the user to update the password for.</param>
        /// <param name="newPassword">Represents the given new password for the user.</param>
        /// <returns>True/False depending on the outcome, or an appropriate error message in case of failure.</returns>
        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
        {
            User? user = await _context.Users.FindAsync(userId);
            if (user is null)
                return new ServiceResponse<bool> { Error = "User not found." };

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true);
        }

        /// <summary>
        /// Recieves the currently authenticated users name identifier from the claims of the JWT.
        /// </summary>
        /// <returns>An integer containing the name identifier.</returns>
        public int GetNameIdFromClaims() =>
            int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Recieves the currently authenticated users email from the claims of the JWT.
        /// </summary>
        /// <returns>A string containing the email.</returns>
        public string? GetUserEmail() =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        /// <summary>
        /// Recieves a user from the database with the given email address.
        /// </summary>
        /// <param name="email">Represents the given email address used to recieve the user.</param>
        /// <returns>A User object.</returns>
        public async Task<User?> GetUserByEmail(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

        /// <summary>
        /// Private method to create a random token.
        /// </summary>
        /// <returns>A string with the token.</returns>
        private static string CreateRandomToken() =>
            Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        /// <summary>
        /// Private method to create a new JSON Web Token with claims based on the given
        /// User object as an input. It uses a symmetric security key with the secret key
        /// from the configuration, and the HMAC SHA512 Signature security algorithm.
        /// The token expires after 2 days.
        /// </summary>
        /// <param name="user">Represents the user to create the JWT for. Meaning the properties of
        /// this object (Id, Email, Role) will be added to the list of claims in the JWT.</param>
        /// <returns>A newly issued JWT for authentication and authorization.</returns>
        private string CreateJWT(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

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
        #endregion
    }
}