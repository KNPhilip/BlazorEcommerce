namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    /// <summary>
    /// Interface for all things regarding Product Type Services.
    /// </summary>
    public interface IProductTypeService
    {
        /// <summary>
        /// Gets all of the Product Types from the database.
        /// </summary>
        /// <returns>A new list of all Product Types.</returns>
        Task<ServiceResponse<List<ProductType>>> GetProductTypesAsync();

        /// <summary>
        /// Adds a new Product Type to the database.
        /// </summary>
        /// <param name="productType"></param>
        /// <returns>A new list of all Product Types, making it possible to inspect the outcome of the method.</returns>
        Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType);

        /// <summary>
        /// Updates the properties of a Product Type.
        /// </summary>
        /// <param name="productType"></param>
        /// <returns>A new list of all Product Types, making it possible to inspect the outcome of the method.</returns>
        Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType);

        /// <summary>
        /// Deletes the Product Type with the given ID from the database.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns>A new list of all Product Types, making it possible to inspect the outcome of the method.</returns>
        Task<ServiceResponse<List<ProductType>>> DeleteProductType(int productTypeId);
    }
}