namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategories()
        {
            var response = await _categoryService.GetCategoriesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAdminCategories()
        {
            var response = await _categoryService.GetAdminCategoriesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> AddCategory(Category category)
        {
            var response = await _categoryService.AddCategoryAsync(category);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> UpdateCategory(Category category)
        {
            var response = await _categoryService.UpdateCategoryAsync(category);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("admin/{categoryId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> DeleteCategory(int categoryId)
        {
            var response = await _categoryService.DeleteCategoryAsync(categoryId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}