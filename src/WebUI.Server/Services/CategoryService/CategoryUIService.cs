using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Interfaces;
using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Services.CategoryService;

public sealed class CategoryUIService(
    ICategoryService categoryService) 
    : ICategoryUIService
{
    public List<Category> Categories { get; set; } = [];
    public List<Category> AdminCategories { get; set; } = [];

    public event Action? OnChange;

    public async Task GetCategories()
    {
        ResponseDto<List<Category>> categories = 
            await categoryService.GetCategoriesAsync();

        Categories = categories.Data ?? [];
    }

    public async Task AddCategory(Category category)
    {
        ResponseDto<List<Category>> result = 
            await categoryService.AddCategoryAsync(category);

        AdminCategories = result.Data ?? [];
        await GetCategories();
        OnChange!.Invoke();
    }

    public Category CreateNewCategory()
    {
        Category newCategory = new()
        {
            IsNew = true,
            Editing = true
        };
        AdminCategories.Add(newCategory);
        OnChange!.Invoke();
        return newCategory;
    }

    public async Task DeleteCategory(int categoryId)
    {
        ResponseDto<List<Category>> result = 
            await categoryService.DeleteCategoryAsync(categoryId);
        AdminCategories = result.Data ?? [];
        await GetCategories();
        OnChange!.Invoke();
    }

    public async Task GetAdminCategories()
    {
        ResponseDto<List<Category>> result = 
            await categoryService.GetAdminCategoriesAsync();

        if (result is not null)
        {
            AdminCategories = result.Data ?? [];
        }
    }

    public async Task UpdateCategory(Category category)
    {
        ResponseDto<List<Category>> result = await categoryService
            .UpdateCategoryAsync(category);
        AdminCategories = result.Data ?? [];
        await GetCategories();
        OnChange!.Invoke();
    }
}
