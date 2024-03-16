using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    public sealed class CategoriesController : ControllerTemplate
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories() =>
            HandleResult(await _categoryService.GetCategoriesAsync());

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Category>>> GetAdminCategories() =>
            HandleResult(await _categoryService.GetAdminCategoriesAsync());

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Category>>> AddCategory(Category category) =>
            HandleResult(await _categoryService.AddCategoryAsync(category));

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Category>>> UpdateCategory(Category category) =>
            HandleResult(await _categoryService.UpdateCategoryAsync(category));

        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Category>>> DeleteCategory(int categoryId) =>
            HandleResult(await _categoryService.DeleteCategoryAsync(categoryId)); 
    }
}
