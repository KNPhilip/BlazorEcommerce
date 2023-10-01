namespace BlazorEcommerce.Shared.Models
{
    /// <summary>
    /// Represents the Product Type entity in the business domain.
    /// </summary>
    public class ProductType
    {
        /// <summary>
        /// Represents the unique identifier for the product type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the name of the product type.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Represents the status of whether the product type is deleted or not. On deletion the
        /// data of old product types is still saved for a period of time in the database
        /// (This is a common approach in most companies because of legal reasons)
        /// - The data is then deleted after x amount of years.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Represents the status of whether or not the product type is currently being edited. This
        /// property is not mapped/saved to the database, its only there for frontend purposes.
        /// </summary>
        [NotMapped]
        public bool Editing { get; set; } = false;

        /// <summary>
        /// Represents the status of whether this product type is "being created" or not. This
        /// property is not mapped/saved to the database, its only there for frontend purposes.
        /// </summary>
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}