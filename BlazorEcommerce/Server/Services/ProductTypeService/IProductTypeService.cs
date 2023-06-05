namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        Task<ServiceResponse<List<ProductType>>> GetProductTypesAsync();
        Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType);
    }
}