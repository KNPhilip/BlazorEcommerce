using Domain.Models;

namespace WebUI.Server.Client.Components.Admin;

public sealed partial class Categories
{
    Category? editingCategory = null;

    protected override async Task OnInitializedAsync()
    {
        await CategoryUIService.GetAdminCategories();
        CategoryUIService.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        CategoryUIService.OnChange += StateHasChanged;
    }

    private void CreateNewCategory()
    {
        editingCategory = CategoryUIService.CreateNewCategory();
    }

    private void EditCategory(Category category)
    {
        category.Editing = true;
        editingCategory = category;
    }

    private async Task UpdateCategory()
    {
        if (editingCategory!.IsNew)
        {
            await CategoryUIService.AddCategory(editingCategory);
        }
        else
        {
            await CategoryUIService.UpdateCategory(editingCategory);
        }

        editingCategory = null;
    }

    private async Task CancelEditing()
    {
        await CategoryUIService.GetAdminCategories();
        editingCategory = null;
    }

    private async Task DeleteCategory(int id)
    {
        await CategoryUIService.DeleteCategory(id);
    }
}
