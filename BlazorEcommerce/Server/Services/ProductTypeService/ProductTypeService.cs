namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly EcommerceContext _context;

        public ProductTypeService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypesAsync()
        {
            var productTypes = await _context.ProductTypes
                .Where(pt => pt.IsDeleted == false)
                .ToListAsync();
            return new ServiceResponse<List<ProductType>>
            {
                Data = productTypes
            };
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            var dbProductType = await GetProductTypeByIdAsync(productType.Id);
            if (dbProductType is null) 
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product Type not found."
                };
            }

            dbProductType.Name = productType.Name;
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        public async Task<ServiceResponse<List<ProductType>>> DeleteProductType(int productTypeId)
        {
            var dbProductType = await GetProductTypeByIdAsync(productTypeId);
            if (dbProductType is null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product Type not found."
                };
            }

            dbProductType.IsDeleted = true;
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        private async Task<ProductType> GetProductTypeByIdAsync(int productTypeId) => await _context.ProductTypes.FindAsync(productTypeId);
    }
}