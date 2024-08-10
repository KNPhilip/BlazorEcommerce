using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Server.Services.ProductTypeService;

public sealed class ProductTypeService(IDbContextFactory<EcommerceContext> contextFactory) 
    : IProductTypeService
{
    private readonly IDbContextFactory<EcommerceContext> _contextFactory = contextFactory;

    public async Task<ResponseDto<List<ProductType>>> GetProductTypesAsync()
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        List<ProductType> productTypes = await dbContext.ProductTypes
            .Where(pt => pt.IsSoftDeleted == false)
            .ToListAsync();

        return ResponseDto<List<ProductType>>.SuccessResponse(productTypes);
    }

    public async Task<ResponseDto<List<ProductType>>> AddProductType(ProductType productType)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        dbContext.ProductTypes.Add(productType);
        await dbContext.SaveChangesAsync();

        return await GetProductTypesAsync();
    }

    public async Task<ResponseDto<List<ProductType>>> UpdateProductType(ProductType productType)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        ProductType? dbProductType = await GetProductTypeByIdAsync(productType.Id);
        if (dbProductType is null)
            return ResponseDto<List<ProductType>>.ErrorResponse("Product Type not found");

        dbProductType.Name = productType.Name;
        await dbContext.SaveChangesAsync();

        return await GetProductTypesAsync();
    }

    public async Task<ResponseDto<List<ProductType>>> DeleteProductType(int productTypeId)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        ProductType? dbProductType = await GetProductTypeByIdAsync(productTypeId);
        if (dbProductType is null)
            return ResponseDto<List<ProductType>>.ErrorResponse("Product Type not found");

        dbProductType.IsSoftDeleted = true;
        await dbContext.SaveChangesAsync();

        return await GetProductTypesAsync();
    }

    private async Task<ProductType?> GetProductTypeByIdAsync(int productTypeId)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        return await dbContext.ProductTypes.FindAsync(productTypeId); 
    }
}
