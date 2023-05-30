using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly EcommerceContext _context;

        public AuthService(EcommerceContext context)
        {
            _context = context;
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
            else if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
            }
            else
            {
                response.Data = "JWT";
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

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()));
        }
    }
}