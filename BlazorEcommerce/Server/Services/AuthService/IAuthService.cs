namespace BlazorEcommerce.Server.Services.AuthService
{
    /// <summary>
    /// Interface for all things regarding Auth Services.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new User with the info of the given input.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>Returns the newly registered user's ID.</returns>
        Task<ServiceResponse<int>> Register(User user, string password);

        /// <summary>
        /// Checks if a user with the given email address is registered.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True/False depending on the response.</returns>
        Task<bool> UserExists(string email);

        /// <summary>
        /// Request to login to the application with a given email address and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>A newly issued JWT for future authentication & authorization.</returns>
        Task<ServiceResponse<string>> Login(string email, string password);

        /// <summary>
        /// Changes the password of the user with the given ID to the new given password.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns>True/False depending on the outcome.</returns>
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);

        /// <summary>
        /// Creates a new password reset token for the user matching the email from the body.
        /// After that, sending the reset token to the users email address.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Instructions on what to do next.</returns>
        Task<ServiceResponse<string>> CreateResetToken(User request);

        /// <summary>
        /// Validates the given password reset token for the given email address.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="resetToken"></param>
        /// <returns>True/False depending on the response,
        /// or an appropriate error message in case of failure.</returns>
        Task<ServiceResponse<bool>> ValidateResetPasswordToken(string email, string resetToken);

        /// <summary>
        /// Resets the password of the user from the database with the given email address,
        /// changing it to the new given password, if the given password reset token is valid.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <param name="resetToken"></param>
        /// <returns>True/False depending on the response.</returns>
        Task<ServiceResponse<bool>> ResetPassword(string email, string newPassword, string resetToken);

        /// <summary>
        /// Recieves the currently authenticated users name identifier from the claims of the JWT.
        /// </summary>
        /// <returns>An integer containing the name identifier.</returns>
        int GetNameIdFromClaims();

        /// <summary>
        /// Recieves the currently authenticated users email from the claims of the JWT.
        /// </summary>
        /// <returns>A string containing the email.</returns>
        string? GetUserEmail();

        /// <summary>
        /// Recieves a user from the database with the given email address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A User object.</returns>
        Task<User?> GetUserByEmail(string email);
    }
}