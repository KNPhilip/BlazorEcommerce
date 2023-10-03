global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel.DataAnnotations;
global using System.Text.Json.Serialization;
global using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Shared
{
    /// <summary>
    /// The ServiceResponse class is a class for handling potential errors, successes, and data.
    /// This is useful for a controller to easily respond with the appropriate status code.
    /// </summary>
    /// <typeparam name="T">Represents the type of data the Service Response contains.</typeparam>
    public class ServiceResponse<T>
    {
        #region Properties
        /// <summary>
        /// Represents the data.
        /// </summary>
        public T? Data { get; set; }
        /// <summary>
        /// Represents the success status of the request.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Represents the potential error message if anything went wrong.
        /// </summary>
        public string Error { get; set; } = string.Empty; 
        #endregion

        #region Methods
        /// <summary>
        /// Method for creating a successful ServiceResponse object
        /// </summary>
        /// <param name="Data">Represents the given data to be set in the object.</param>
        /// <returns>A new successful ServiceResponse object with the appropriate data.</returns>
        public static ServiceResponse<T> SuccessResponse(T Data) =>
            new() { Data = Data, Success = true, Error = string.Empty };

        /// <summary>
        /// Method for creating a failed ServiceResponse object.
        /// </summary>
        /// <param name="message">Represents the error message to be set in the object.</param>
        /// <returns>A failed ServiceResponse object with the appropriate error message.</returns>
        public static ServiceResponse<T> ErrorResponse(string message) =>
            new() { Success = false, Error = message }; 
        #endregion
    }
}