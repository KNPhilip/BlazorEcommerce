﻿namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    /// <summary>
    /// Implementation class of IProductTypeService.
    /// </summary>
    public class ProductTypeService : IProductTypeService
    {
        /// <summary>
        /// Instance of the EcommerceContext (EF Data Context)
        /// </summary>
        private readonly EcommerceContext _context;

        public ProductTypeService(EcommerceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all of the Product Types from the database.
        /// </summary>
        /// <returns>A new list of all Product Types.</returns>
        public async Task<ServiceResponse<List<ProductType>>> GetProductTypesAsync()
        {
            List<ProductType> productTypes = await _context.ProductTypes
                .Where(pt => pt.IsDeleted == false)
                .ToListAsync();

            return ServiceResponse<List<ProductType>>.SuccessResponse(productTypes);
        }

        /// <summary>
        /// Adds a new Product Type to the database.
        /// </summary>
        /// <param name="productType"></param>
        /// <returns>A new list of all Product Types, making it possible to inspect the outcome of the method.</returns>
        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        /// <summary>
        /// Updates the properties of a Product Type.
        /// </summary>
        /// <param name="productType"></param>
        /// <returns>A new list of all Product Types, making it possible to inspect the outcome of the method, or an error if the Product Type was not found.</returns>
        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            ProductType? dbProductType = await GetProductTypeByIdAsync(productType.Id);
            if (dbProductType is null)
                return new() { Error = "Product Type not found" };

            dbProductType.Name = productType.Name;
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        /// <summary>
        /// Deletes the Product Type with the given ID from the database.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns>A new list of all Product Types, making it possible to inspect the outcome of the method, or an error if the Product Type was not found.</returns>
        public async Task<ServiceResponse<List<ProductType>>> DeleteProductType(int productTypeId)
        {
            ProductType? dbProductType = await GetProductTypeByIdAsync(productTypeId);
            if (dbProductType is null)
                return new() { Error = "Product Type not found" };

            dbProductType.IsDeleted = true;
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        /// <summary>
        /// Extracted method to recieve a Product Type from the database with the given ID.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns>The ProductType with the given ID, if any.</returns>
        private async Task<ProductType?> GetProductTypeByIdAsync(int productTypeId) => 
            await _context.ProductTypes.FindAsync(productTypeId);
    }
}