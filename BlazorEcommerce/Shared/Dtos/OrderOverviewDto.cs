namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object representing the overview of an order.
    /// </summary>
    public class OrderOverviewDto
    {
        /// <summary>
        /// Represents the unique identifier for the order.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the date that the order was made.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Represents the total price of the order.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Represents the name(s) of the product(s) that the order is containing.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Represents the image url to represent the order.
        /// </summary>
        public string ProductImageUrl { get; set; }

        /// <summary>
        /// Represents the list of images of the order. However,
        /// only the first image in this list will be used to represent the order.
        /// </summary>
        public List<Image> Images { get; set; }
    }
}