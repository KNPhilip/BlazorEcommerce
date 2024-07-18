using Domain.Models;

namespace WebUI.Server.Client.Components.Shared;

public sealed partial class ProductList
{
    protected override void OnInitialized()
    {
        ProductUIService.OnProductsChanged += StateHasChanged;
    }

    public void Dispose()
    {
        ProductUIService.OnProductsChanged -= StateHasChanged;
    }

    private static string GetPriceText(Product product)
    {
        List<ProductVariant> variants = product.Variants;

        if (variants.Count == 0)
        {
            return string.Empty;
        }
        else if (variants.Count == 1)
        {
            return $"${variants[0].Price}";
        }

        decimal minPrice = variants.Min(v => v.Price);
        return $"Starting at ${minPrice}";
    }
}
