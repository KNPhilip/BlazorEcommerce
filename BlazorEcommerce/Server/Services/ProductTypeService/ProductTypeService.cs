namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly EcommerceContext _context;

        public ProductTypeService(EcommerceContext context)
        {
            _context = context;
        }
    }
}