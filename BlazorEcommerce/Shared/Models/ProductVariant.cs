namespace BlazorEcommerce.Shared.Models
{
    /// <summary>
    /// Represents the Product Variant entity in the business domain.
    /// </summary>
    public class ProductVariant
    {
        /// <summary>
        /// Represents the product that the variant belongs to.
        /// </summary>
        [JsonIgnore]
        public Product? Product { get; set; }

        /// <summary>
        /// Represents the unique identifier of the product that the variant belongs to.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Represents the product type of the product variant.
        /// </summary>
        public ProductType? ProductType { get; set; }

        /// <summary>
        /// Represents the unique identifier of the product type of the product variant.
        /// </summary>
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Represents the price of the product variant.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Represents the original price of the product variant. This is only set when you
        /// want to display a discount. Meaning the price property will be the
        /// discount price, and this will be the old price.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Represents the status of whether the product variant
        /// should be visible for regular users or not.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Represents the status of whether the product variant is deleted or not. On deletion the
        /// data of old product variants is still saved for a period of time in the database
        /// (This is a common approach in most companies because of legal reasons)
        /// - The data is then deleted after x amount of years.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Represents the status of whether or not the product variant is currently being edited. This
        /// property is not mapped/saved to the database, its only there for frontend purposes.
        /// </summary>
        [NotMapped]
        public bool Editing { get; set; } = false;

        /// <summary>
        /// Represents the status of whether this product variant is "being created" or not. This
        /// property is not mapped/saved to the database, its only there for frontend purposes.
        /// </summary>
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}