using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public sealed class ProductTypeService : IProductTypeService
    {
        private readonly EcommerceContext _context;

        public ProductTypeService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<List<ProductType>>> GetProductTypesAsync()
        {
            List<ProductType> productTypes = await _context.ProductTypes
                .Where(pt => pt.IsSoftDeleted == false)
                .ToListAsync();

            return ResponseDto<List<ProductType>>.SuccessResponse(productTypes);
        }

        public async Task<ResponseDto<List<ProductType>>> AddProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        public async Task<ResponseDto<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            ProductType? dbProductType = await GetProductTypeByIdAsync(productType.Id);
            if (dbProductType is null)
                return ResponseDto<List<ProductType>>.ErrorResponse("Product Type not found");

            dbProductType.Name = productType.Name;
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        public async Task<ResponseDto<List<ProductType>>> DeleteProductType(int productTypeId)
        {
            ProductType? dbProductType = await GetProductTypeByIdAsync(productTypeId);
            if (dbProductType is null)
                return ResponseDto<List<ProductType>>.ErrorResponse("Product Type not found");

            dbProductType.IsSoftDeleted = true;
            await _context.SaveChangesAsync();

            return await GetProductTypesAsync();
        }

        private async Task<ProductType?> GetProductTypeByIdAsync(int productTypeId) =>
            await _context.ProductTypes.FindAsync(productTypeId); 
    }
}
