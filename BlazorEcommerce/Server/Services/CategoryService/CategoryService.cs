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

        public async Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category)
        {
            category.Editing = category.IsNew = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategoriesAsync();
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c => !c.IsDeleted).ToListAsync();

            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c => !c.IsDeleted && c.Visible).ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories 
            };
        }
    }
}