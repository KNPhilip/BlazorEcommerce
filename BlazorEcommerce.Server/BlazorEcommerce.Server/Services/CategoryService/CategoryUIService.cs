using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Services.CategoryService
{
    public sealed class CategoryUIService(HttpClient http) 
        : ICategoryUIService
    {
        public List<Category> Categories { get; set; } = [];

        public event Action? OnChange;

        public async Task GetCategories()
        {
            Categories = await http
                .GetFromJsonAsync<List<Category>>
                    ("api/v1/categories") ?? [];
        }
    }
}
