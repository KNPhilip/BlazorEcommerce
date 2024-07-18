namespace WebUI.Server.Client.Components.Admin;

public sealed partial class Products
{
    protected override async Task OnInitializedAsync()
    {
        await ProductUIService.GetAdminProducts();
    }

    private void EditProduct(int productId)
    {
        NavigationManager.NavigateTo($"admin/product/{productId}");
    }

    private void CreateProduct()
    {
        NavigationManager.NavigateTo($"admin/product");
    }
}
