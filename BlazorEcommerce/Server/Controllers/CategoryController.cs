namespace BlazorEcommerce.Server.Controllers
{
    /// <summary>
    /// Category Controller - Contains all endpoints regarding categories.
    /// </summary>
    public class CategoryController : ControllerTemplate
    {
        /// <summary>
        /// ICategoryService instance. This accesses the implementation class of the CategoryService through the IoC container.
        /// </summary>
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
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
        /// <param name="category"></param>
        /// <returns>A list of all categories.</returns>
        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> AddCategory(Category category) =>
            HandleResult(await _categoryService.AddCategoryAsync(category));

        /// <summary>
        /// Endpoint for administrators to update a category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>A list of all categories.</returns>
        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> UpdateCategory(Category category) =>
            HandleResult(await _categoryService.UpdateCategoryAsync(category));

        /// <summary>
        /// Endpoint for administrators to delete the category from the database with the given ID.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>A list of all categories.</returns>
        [HttpDelete("admin/{categoryId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> DeleteCategory(int categoryId) =>
            HandleResult(await _categoryService.DeleteCategoryAsync(categoryId));
    }
}