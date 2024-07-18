using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Client.Components.Layout;

public partial class Search
{
    private string searchTerm = string.Empty;
    private List<string> suggestions = [];
    protected ElementReference searchInput;

    protected sealed override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await searchInput.FocusAsync();
        }
    }

    private void SearchProducts()
    {
        NavigationManager.NavigateTo($"search/{searchTerm}/1");
    }

    private async Task HandleSearch(KeyboardEventArgs args)
    {
        if (args.Key is null || args.Key.Equals("Enter"))
        {
            SearchProducts();
        }
        else if (searchTerm.Length > 1)
        {
            suggestions = await ProductUIService.GetProductSearchSuggestions(searchTerm);
        }
    }
}
