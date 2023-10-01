namespace BlazorEcommerce.Shared.Models
{
    /// <summary>
    /// Represents the Image entity in the business domain.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Represents the unique identifier for the image.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the base64 string of the image.
        /// </summary>
        public string Data { get; set; } = string.Empty;
    }
}