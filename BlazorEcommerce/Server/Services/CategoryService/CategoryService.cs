using BlazorEcommerce.Shared.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BlazorEcommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommerceContext _context;

        public CategoryService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories 
            };
        }
    }
}