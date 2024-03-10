using BlazorEcommerce.Domain.Models;
using System.Net.Http.Json;

namespace BlazorEcommerce.Server.Client.Services.CategoryService
{
    public sealed class CategoryUIService(HttpClient http) 
        : ICategoryUIService
    {
        public List<Category> Categories { get; set; } = [];
        public List<Category> AdminCategories { get; set; } = [];

        public event Action? OnChange;

        public async Task DeleteCategory(int categoryId)
        {
            HttpResponseMessage response = await http
                .DeleteAsync($"api/v1/categories/{categoryId}");
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<List<Category>>())!;
            await GetCategories();
            OnChange!.Invoke();
        }

        public async Task UpdateCategory(Category category)
        {
            HttpResponseMessage response = await http
                .PutAsJsonAsync("api/v1/categories", category);
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<List<Category>>())!;
            await GetCategories();
            OnChange!.Invoke();
        }

        public async Task AddCategory(Category category)
        {
            HttpResponseMessage response = await http
                .PostAsJsonAsync("api/v1/categories", category);
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<List<Category>>())!;
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

        public async Task GetAdminCategories()
        {
            List<Category>? response = await http
                .GetFromJsonAsync<List<Category>>
                    ("api/v1/categories/admin");

            if (response is not null)
            {
                AdminCategories = response;
            }
        }

        public async Task GetCategories()
        {
            Categories = await http
                .GetFromJsonAsync<List<Category>>
                    ("api/v1/categories") ?? [];
        }
    }
}
