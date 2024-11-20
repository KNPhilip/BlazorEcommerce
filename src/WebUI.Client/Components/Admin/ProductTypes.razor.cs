using Domain.Models;

namespace WebUI.Client.Components.Admin;

public sealed partial class ProductTypes
{
    ProductType? editingProductType = null;

    protected override async Task OnInitializedAsync()
    {
        await ProductTypeUIService.GetProductTypes();
        ProductTypeUIService.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        ProductTypeUIService.OnChange -= StateHasChanged;
    }

    private void EditProductType(ProductType productType)
    {
        productType.Editing = true;
        editingProductType = productType;
    }

    private void CreateNewProductType()
    {
        editingProductType = ProductTypeUIService.CreateNewProductType();
    }

    private async Task CancelEditing()
    {
        await ProductTypeUIService.GetProductTypes();
        editingProductType = null;
    }

    private async Task UpdateProductType()
    {
        if (editingProductType!.IsNew)
        {
            await ProductTypeUIService.AddProductType(editingProductType);
        }
        else
        {
            await ProductTypeUIService.UpdateProductType(editingProductType);
        }
        editingProductType = null;
    }

    private async Task DeleteProductType(int productTypeId)
    {
        await ProductTypeUIService.DeleteProductType(productTypeId);
    }
}
