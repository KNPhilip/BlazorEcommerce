namespace BlazorEcommerce.Server.Controllers
{
    public class CategoryController : ControllerTemplate
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategories() =>
            HandleResult(await _categoryService.GetCategoriesAsync());

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAdminCategories() =>
            HandleResult(await _categoryService.GetAdminCategoriesAsync());

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> AddCategory(Category category) =>
            HandleResult(await _categoryService.AddCategoryAsync(category));

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> UpdateCategory(Category category) =>
            HandleResult(await _categoryService.UpdateCategoryAsync(category));

        [HttpDelete("admin/{categoryId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> DeleteCategory(int categoryId) =>
            HandleResult(await _categoryService.DeleteCategoryAsync(categoryId));
    }
}