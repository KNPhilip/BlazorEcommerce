namespace WebUI.Server.Client.Components.Shared;

public sealed partial class FeaturedProducts
{
    protected override void OnInitialized()
    {
        ProductUIService.OnProductsChanged += StateHasChanged;
    }

    public void Dispose()
    {
        ProductUIService.OnProductsChanged -= StateHasChanged;
    }
}
