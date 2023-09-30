global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel.DataAnnotations;
global using System.Text.Json.Serialization;
global using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Shared
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;

        public static ServiceResponse<T> SuccessResponse(T Data) =>
            new() { Data = Data, Success = true, Error = string.Empty };

        public static ServiceResponse<T> ErrorResponse(string message) =>
            new() { Success = false, Error = message };
    }
}