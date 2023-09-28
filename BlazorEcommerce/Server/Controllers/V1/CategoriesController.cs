namespace BlazorEcommerce.Server.Controllers.V1
{
    /// <summary>
    /// Category Controller - Contains all endpoints regarding categories.
    /// </summary>
    public class CategoriesController : ControllerTemplate
    {
        /// <summary>
        /// ICategoryService field. Used to access the Category Services.
        /// </summary>
        private readonly ICategoryService _categoryService;

        /// <param name="categoryService">ICategoryService instance to be passed on to the
        /// field, containing the correct implementation class through the IoC container.</param>
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Endpoint to get all categories from the database.
        /// </summary>
        /// <returns>A list of all the categories, or an error message in case of failure.</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategories() =>
            HandleResult(await _categoryService.GetCategoriesAsync());

        /// <summary>
        /// Endpoint for administrators to recieve all categories - even those marked as invisible.
        /// </summary>
        /// <returns>A list of the appropriate categories, or an error message in case of failure.</returns>
        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAdminCategories() =>
            HandleResult(await _categoryService.GetAdminCategoriesAsync());

        /// <summary>
        /// Endpoint for administrators to create a new category.
        /// </summary>
        /// <param name="category">Represents the given category to be added.</param>
        /// <returns>A list of all categories.</returns>
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> AddCategory(Category category) =>
            HandleResult(await _categoryService.AddCategoryAsync(category));

        /// <summary>
        /// Endpoint for administrators to update a category.
        /// </summary>
        /// <param name="category">Represents the given category, used to update the existing one.
        /// The existing one is found by taking the given id within the given category and finding
        /// the matching category from the database.</param>
        /// <returns>A list of all categories.</returns>
        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> UpdateCategory(Category category) =>
            HandleResult(await _categoryService.UpdateCategoryAsync(category));

        /// <summary>
        /// Endpoint for administrators to delete the category from the database with the given ID.
        /// </summary>
        /// <param name="categoryId">Represents the given ID of the category to be deleted from the database.</param>
        /// <returns>A list of all categories.</returns>
        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> DeleteCategory(int categoryId) =>
            HandleResult(await _categoryService.DeleteCategoryAsync(categoryId));
    }
}