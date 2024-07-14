using Domain.Dtos;
using Domain.Interfaces;
using Domain.Models;

namespace WebUI.Server.Services.ProductTypeService;

public sealed class ProductTypeUIService(
    IProductTypeService productTypeService) 
    : IProductTypeUIService
{
    public List<ProductType> ProductTypes { get; set; } = [];

    public event Action? OnChange;

    public async Task AddProductType(ProductType productType)
    {
        productType.Editing = productType.IsNew = false;
        ResponseDto<List<ProductType>> result = 
            await productTypeService.AddProductType(productType);
        ProductTypes = result.Data ?? [];
        OnChange!.Invoke();
    }

    public ProductType CreateNewProductType()
    {
        ProductType newProductType = new()
        {
            IsNew = true,
            Editing = true,
        };

        ProductTypes.Add(newProductType);
        OnChange!.Invoke();
        return newProductType;
    }

    public async Task DeleteProductType(int productTypeId)
    {
        ResponseDto<List<ProductType>> result = 
            await productTypeService.DeleteProductType(productTypeId);
        ProductTypes = result.Data ?? [];
        OnChange!.Invoke();
    }

    public async Task GetProductTypes()
    {
        ResponseDto<List<ProductType>> result = 
            await productTypeService.GetProductTypesAsync();
        ProductTypes = result.Data ?? [];
    }

    public async Task UpdateProductType(ProductType productType)
    {
        ResponseDto<List<ProductType>> result =
            await productTypeService.UpdateProductType(productType);
        ProductTypes = result.Data ?? [];
        OnChange!.Invoke();
    }
}
