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
            Category? dbCategory = await GetCategoryById(categoryId);
            if (dbCategory is null)
                return new ServiceResponse<List<Category>> { Error = "Category not found." }; 
            
            dbCategory.IsDeleted = true;
            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category)
        {
            Category? dbCategory = await GetCategoryById(category.Id);
            if (dbCategory is null)
                return new ServiceResponse<List<Category>> { Error = "Category not found." };

            // TODO Might be wise to add Automapper / Mapster here
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
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsDeleted).ToListAsync();

            return ServiceResponse<List<Category>>.SuccessResponse(categories);
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsDeleted && c.Visible).ToListAsync();

            return ServiceResponse<List<Category>>.SuccessResponse(categories);
        }

        private async Task<Category?> GetCategoryById(int id) => 
            await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }
}