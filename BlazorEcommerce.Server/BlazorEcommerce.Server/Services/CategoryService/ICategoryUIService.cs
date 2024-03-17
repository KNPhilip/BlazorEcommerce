using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Services.CategoryService;

public interface ICategoryUIService
{
    event Action OnChange;
    List<Category> Categories { get; set; }
    Task GetCategories();
}
