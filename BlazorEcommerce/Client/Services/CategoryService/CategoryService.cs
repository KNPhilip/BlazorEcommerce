namespace BlazorEcommerce.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        public List<Category> Categories { get; set; } = new();
        public List<Category> AdminCategories { get; set; } = new();

        public event Action? OnChange;

        public async Task DeleteCategory(int categoryId)
        {
            var response = await _http.DeleteAsync($"api/v1/categories/{categoryId}");
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<List<Category>>())!;
            await GetCategories();
            OnChange!.Invoke();
        }

        public async Task UpdateCategory(Category category)
        {
            var response = await _http.PutAsJsonAsync("api/v1/categories", category);
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<List<Category>>())!;
            await GetCategories();
            OnChange!.Invoke();
        }

        public async Task AddCategory(Category category)
        {
            var response = await _http.PostAsJsonAsync("api/v1/categories", category);
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
            List<Category>? response = await _http.GetFromJsonAsync<List<Category>>("api/v1/categories/admin");
            if (response is not null)
            {
                AdminCategories = response;
            }
        }

        public async Task GetCategories()
        {
            Categories = await _http.GetFromJsonAsync<List<Category>>("api/v1/categories") ?? [];
        }
    }
}