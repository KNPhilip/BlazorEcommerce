namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly EcommerceContext _context;

        public ProductTypeService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypesAsync()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>>
            {
                Data = productTypes
            };
        }
    }
}