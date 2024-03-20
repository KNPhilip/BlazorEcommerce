using BlazorEcommerce.Domain.Interfaces;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.CategoryService;

public sealed class CategoryUIService(EcommerceContext _context) 
    : ICategoryUIService
{
    public List<Category> Categories { get; set; } = [];
    public List<Category> AdminCategories 
    { 
        get => throw new NotImplementedException(); 
        set => throw new NotImplementedException(); 
    }

    public event Action? OnChange;

    public async Task GetCategories()
    {
        List<Category>? categories = await _context.Categories
            .Where(c => !c.IsSoftDeleted && c.Visible).ToListAsync();

        Categories = categories ?? [];
    }

    public Task AddCategory(Category category) => 
        throw new NotImplementedException();

    public Category CreateNewCategory() =>
        throw new NotImplementedException();

    public Task DeleteCategory(int categoryId) =>
        throw new NotImplementedException();

    public Task GetAdminCategories() =>
        throw new NotImplementedException();

    public Task UpdateCategory(Category category) =>
        throw new NotImplementedException();
}
