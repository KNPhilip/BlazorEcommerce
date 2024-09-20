namespace WebUI.Client.Components.Layout;

public sealed partial class CartCounter
{
    private int cartItemsCount;

    protected override void OnInitialized()
    {
        CartUIService.OnChange += StateHasChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        cartItemsCount = await GetCartItemsCountAsync();
    }

    public void Dispose()
    {
        CartUIService.OnChange -= StateHasChanged;
    }

    private async Task<int> GetCartItemsCountAsync()
    {
        return await LocalStorage.GetItemAsync<int>("cartItemsCount");
    }
}
