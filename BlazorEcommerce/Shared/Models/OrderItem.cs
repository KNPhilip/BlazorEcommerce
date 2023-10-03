namespace BlazorEcommerce.Shared.Models
{
    /// <summary>
    /// Represents the Order Item entity in the business domain.
    /// </summary>
    public class OrderItem
    {
        #region Properties
        /// <summary>
        /// Represents the order that the order item belongs to.
        /// </summary>
        public required Order Order { get; set; }

        /// <summary>
        /// Represents the unique identifier of the order that the order item belongs to.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Represents the product that the order item contains of.
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Represents the unique identifier of the product that the order item contains of.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Represents the product type of the product within the order item.
        /// </summary>
        public ProductType? ProductType { get; set; }

        /// <summary>
        /// Represents the unique identifier for the product type of the
        /// product within the order item.
        /// </summary>
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Represents the quantity of the order item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Represents the total price of the order item (price * quantity)
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } 
        #endregion
    }
}