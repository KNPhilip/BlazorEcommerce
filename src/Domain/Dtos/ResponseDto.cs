namespace Domain.Dtos;

/// <summary>
/// This is a special class for handling potential errors, successes, and data.
/// This is useful for a controller to easily respond with the appropriate status code.
/// </summary>
/// <typeparam name="T">Represents the type of data the response DTO contains.</typeparam>
public sealed class ResponseDto<T>
{
    public T? Data { get; set; }
    public required bool Success { get; set; }
    public string? Error { get; set; } 

    /// <summary>
    /// Creates a successful ResponseDto object containing the given data.
    /// </summary>
    /// <param name="Data">Represents the given data to be set in the object.</param>
    /// <returns>A new ResponseDto object with data.</returns>
    public static ResponseDto<T> SuccessResponse(T Data)
    {
        return new() { Data = Data, Success = true, Error = string.Empty };
    }

    /// <summary>
    /// Creates a failed ResponseDto object containing the given error message.
    /// </summary>
    /// <param name="message">Represents the error message to be set in the object.</param>
    /// <returns>A failed ResponseDto object with an error message.</returns>
    public static ResponseDto<T> ErrorResponse(string message)
    {
        return new() { Success = false, Error = message }; 
    }
}
