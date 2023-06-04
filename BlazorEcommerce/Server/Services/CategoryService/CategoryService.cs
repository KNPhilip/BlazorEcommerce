namespace BlazorEcommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommerceContext _context;

        public CategoryService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int categoryId)
        {
            var dbCategory = await GetCategoryById(categoryId);
            if (dbCategory is null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            dbCategory.IsDeleted = true;
            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category)
        {
            var dbCategory = await GetCategoryById(category.Id);
            if (dbCategory is null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
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

        private async Task<Category> GetCategoryById(int id) => await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }
}