using Domain.Dtos;
using Domain.Models;

namespace WebUI.Server.Services.ProductTypeService;

public interface IProductTypeService
{
    Task<ResponseDto<List<ProductType>>> GetProductTypesAsync();
    Task<ResponseDto<List<ProductType>>> AddProductType(ProductType productType);
    Task<ResponseDto<List<ProductType>>> UpdateProductType(ProductType productType);
    Task<ResponseDto<List<ProductType>>> DeleteProductType(int productTypeId);
}
