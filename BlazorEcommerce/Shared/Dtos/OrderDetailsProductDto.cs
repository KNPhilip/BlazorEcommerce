namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object for representing each product within an order.
    /// </summary>
    public class OrderDetailsProductDto
    {
        /// <summary>
        /// Represents the unique identifier of the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Represents the title of the product.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Represents the name of the product type of the product.
        /// </summary>
        public required string ProductType { get; set; }

        /// <summary>
        /// Represents the image url (if any) of the product.
        /// </summary>
        public required string ImageUrl { get; set; }

        /// <summary>
        /// Represents the images (if any) of the product.
        /// </summary>
        public List<Image> Images { get; set; } = new List<Image>();

        /// <summary>
        /// Represents the quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Represents the total price of the product.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}