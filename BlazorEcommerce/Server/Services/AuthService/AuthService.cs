namespace BlazorEcommerce.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly EcommerceContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            EcommerceContext context, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
            }
            else
            {
                response.Data = CreateJWT(user);
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }
            
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = user.Id,
                Message = "Registration Successful!"
            };
        }

        public async Task<ServiceResponse<string>> CreateResetToken(User request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            var response = new ServiceResponse<string>();
            if (user == null)
            {
                response.Success = false;
                response.Message = "There are no registered users with the email address " + request.Email + ".";
                return response;
            }

            if (!String.IsNullOrEmpty(user.PasswordResetToken))
            {
                response.Success = false;
                response.Message = "There is an open reset request already! Please check your inbox.";
                return response;
            }

            var tok = Guid.NewGuid();
            user.PasswordResetToken = tok.ToString();

            await _context.SaveChangesAsync();
            response.Data = tok.ToString();
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<bool>> ResetPassword(string email, string newPassword, string resetToken)
        {
            var response = new ServiceResponse<bool>();
            if (!await UserExists(email))
            {
                response.Message = "There's no registered users with the email " + email + ".";
                response.Success = false;
                return response;
            }
            else if (!await _context.Users.AnyAsync(user => user.PasswordResetToken.ToLower().Equals(resetToken.ToLower())))
            {
                response.Message = "Wrong reset token!";
                response.Success = false;
                return response;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                response.Message = "User with email " + email + " not found.";
                response.Success = false;
                return response;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordHash = string.Empty;

            await _context.SaveChangesAsync();
            response.Message = "Password reset successfully.";
            response.Success = true;
            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()));
        }

        private string CreateJWT(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

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
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> 
            {
                Data = true,
                Message = "Password has been changed."
            };
        }

        public int GetNameIdFromClaims() =>
            int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        public string GetUserEmail() =>
            _httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.Name);

        public async Task<User> GetUserByEmail(string email) =>
            await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Equals(email));
    }
}