using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Server.Services.CategoryService;

public sealed class CategoryService(IDbContextFactory<EcommerceContext> contextFactory) 
    : ICategoryService
{
    private readonly IDbContextFactory<EcommerceContext> _contextFactory = contextFactory;

    public async Task<ResponseDto<List<Category>>> GetCategoriesAsync()
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        List<Category> categories = await dbContext.Categories
            .Where(c => !c.IsSoftDeleted && c.Visible).ToListAsync();

        return ResponseDto<List<Category>>.SuccessResponse(categories);
    }

    public async Task<ResponseDto<List<Category>>> GetAdminCategoriesAsync()
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        List<Category> categories = await dbContext.Categories
            .Where(c => !c.IsSoftDeleted).ToListAsync();

        return ResponseDto<List<Category>>.SuccessResponse(categories);
    }

    public async Task<ResponseDto<List<Category>>> AddCategoryAsync(Category category)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        category.Editing = category.IsNew = false;
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();
        return await GetAdminCategoriesAsync();
    }

    public async Task<ResponseDto<List<Category>>> UpdateCategoryAsync(Category category)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        Category? dbCategory = await GetCategoryById(category.Id);
        if (dbCategory is null)
        {
            return ResponseDto<List<Category>>.ErrorResponse("Category not found.");
        }

        // TODO Might be wise to add Automapper / Mapster here
        dbCategory.Name = category.Name;
        dbCategory.Url = category.Url;
        dbCategory.Visible = category.Visible;

        await dbContext.SaveChangesAsync();

        return await GetAdminCategoriesAsync();
    }

    public async Task<ResponseDto<List<Category>>> DeleteCategoryAsync(int categoryId)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        Category? dbCategory = await GetCategoryById(categoryId);
        if (dbCategory is null)
        {
            return ResponseDto<List<Category>>.ErrorResponse("Category not found.");
        }

        dbCategory.IsSoftDeleted = true;
        await dbContext.SaveChangesAsync();

        return await GetAdminCategoriesAsync();
    }

    private async Task<Category?> GetCategoryById(int id)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id); 
    }
}
