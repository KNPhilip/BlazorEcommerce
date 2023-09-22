using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly EcommerceContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailService;

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

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user is null)
                return new ServiceResponse<string> { Error = "User not found." };
            else if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return new ServiceResponse<string> { Error = "Incorrect password" };

            return ServiceResponse<string>.SuccessResponse(CreateJWT(user));
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await UserExists(user.Email))
                return new ServiceResponse<int> { Error = "User already exists." };
            
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return ServiceResponse<int>.SuccessResponse(user.Id);
        }

        public async Task<ServiceResponse<string>> CreateResetToken(User request)
        {
            var user = await GetUserByEmail(request.Email);
            if (user is null)
                return new ServiceResponse<string> { Error =
                    $"There are no registered users with the email address {request.Email}" };

            if (!String.IsNullOrEmpty(user.PasswordResetToken) && DateTime.Now < user.ResetTokenExpires)
                return new ServiceResponse<string> { Error =
                    "There is an open reset request already! Please check your inbox or try again later"
                };

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddHours(2);
            await _context.SaveChangesAsync();

            var mail = new SendMailDto
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

        public async Task<ServiceResponse<bool>> ResetPassword(string email, string newPassword, string resetToken)
        {
            ServiceResponse<bool> response = await ValidateResetPasswordToken(email, resetToken);
            if(!response.Success) return response;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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

        public async Task<ServiceResponse<bool>> ValidateResetPasswordToken(string email, string resetToken)
        {
            if (!await UserExists(email))
                return new ServiceResponse<bool> { Error = $"There is no registered users with the email {email}." };
            else if (!await _context.Users.AnyAsync(user => user.PasswordResetToken.ToLower().Equals(resetToken.ToLower())))
                return new ServiceResponse<bool> { Error = "Invalid reset token" };

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                return new ServiceResponse<bool> { Error = $"User with email {email} not found." };
            else if (user.ResetTokenExpires < DateTime.Now)
                return new ServiceResponse<bool> { Error = "This reset token has expired.." };

            return ServiceResponse<bool>.SuccessResponse(true);
        }

        public async Task<bool> UserExists(string email) =>
            await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()));

        private string CreateJWT(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddDays(2)
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
                return new ServiceResponse<bool> { Error = "User not found." };

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true);
        }

        public int GetNameIdFromClaims() =>
            int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public string GetUserEmail() =>
            _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        public async Task<User> GetUserByEmail(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

        private static string CreateRandomToken() =>
            Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }
}