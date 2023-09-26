namespace BlazorEcommerce.Server.Services.CategoryService
{
    /// <summary>
    /// Implementation class of ICategoryService.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        /// <summary>
        /// Instance of EcommerceContext (EF Data Context)
        /// </summary>
        private readonly EcommerceContext _context;

        public CategoryService(EcommerceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Recieves a list of all available categories from the database.
        /// </summary>
        /// <returns>A list of Category.</returns>
        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsDeleted && c.Visible).ToListAsync();

            return ServiceResponse<List<Category>>.SuccessResponse(categories);
        }

        /// <summary>
        /// Recieves a list of all admin categories (meaning including the ones
        /// marked as invisible) from the database.
        /// </summary>
        /// <returns>A list of Category.</returns>
        public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
        {
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsDeleted).ToListAsync();

            return ServiceResponse<List<Category>>.SuccessResponse(categories);
        }

        /// <summary>
        /// Adds the given Category to the database.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>A list of all categories, also those marked as invisible.</returns>
        public async Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category)
        {
            category.Editing = category.IsNew = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategoriesAsync();
        }

        /// <summary>
        /// Updates a category in the database.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>A list of all categories, also those marked as invisible.</returns>
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

        /// <summary>
        /// Deletes the category from the database matching the given ID.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>A list of all categories, also those marked as invisible.</returns>
        public async Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int categoryId)
        {
            Category? dbCategory = await GetCategoryById(categoryId);
            if (dbCategory is null)
                return new ServiceResponse<List<Category>> { Error = "Category not found." };

            dbCategory.IsDeleted = true;
            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        /// <summary>
        /// Extracted method to recieve a category from the database with the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Category.</returns>
        private async Task<Category?> GetCategoryById(int id) => 
            await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }
}