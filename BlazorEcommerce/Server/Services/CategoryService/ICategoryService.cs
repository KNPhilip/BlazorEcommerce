namespace BlazorEcommerce.Server.Services.CategoryService
{
    /// <summary>
    /// Interface for all things regarding Category Services.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Recieves a list of all available categories from the database.
        /// </summary>
        /// <returns>A list of Category.</returns>
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();

        /// <summary>
        /// Recieves a list of all admin categories (meaning including the ones
        /// marked as invisible) from the database.
        /// </summary>
        /// <returns>A list of Category.</returns>
        Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync();

        /// <summary>
        /// Adds the given Category to the database.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>A list of Category.</returns>
        Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category);

        /// <summary>
        /// Updates a category in the database.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>A list of Category.</returns>
        Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category);

        /// <summary>
        /// Deletes the category from the database matching the given ID.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>A list of Category.</returns>
        Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int categoryId);
    }
}