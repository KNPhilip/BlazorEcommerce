﻿global using System.ComponentModel.DataAnnotations.Schema;
global using System.Text.Json.Serialization;

namespace BlazorEcommerce.Shared
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}