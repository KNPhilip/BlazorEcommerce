using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.CategoryService
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly EcommerceContext _context; 

        public CategoryService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<List<Category>>> GetCategoriesAsync()
        {
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsSoftDeleted && c.Visible).ToListAsync();

            return ResponseDto<List<Category>>.SuccessResponse(categories);
        }

        public async Task<ResponseDto<List<Category>>> GetAdminCategoriesAsync()
        {
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsSoftDeleted).ToListAsync();

            return ResponseDto<List<Category>>.SuccessResponse(categories);
        }

        public async Task<ResponseDto<List<Category>>> AddCategoryAsync(Category category)
        {
            category.Editing = category.IsNew = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategoriesAsync();
        }

        public async Task<ResponseDto<List<Category>>> UpdateCategoryAsync(Category category)
        {
            Category? dbCategory = await GetCategoryById(category.Id);
            if (dbCategory is null)
                return ResponseDto<List<Category>>.ErrorResponse("Category not found.");

            // TODO Might be wise to add Automapper / Mapster here
            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        public async Task<ResponseDto<List<Category>>> DeleteCategoryAsync(int categoryId)
        {
            Category? dbCategory = await GetCategoryById(categoryId);
            if (dbCategory is null)
                return ResponseDto<List<Category>>.ErrorResponse("Category not found.");

            dbCategory.IsSoftDeleted = true;
            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        private async Task<Category?> GetCategoryById(int id) =>
            await _context.Categories.FirstOrDefaultAsync(c => c.Id == id); 
    }
}
