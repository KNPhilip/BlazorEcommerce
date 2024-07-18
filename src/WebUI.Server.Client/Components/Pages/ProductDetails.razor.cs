using Domain.Models;
using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Client.Components.Pages;

public sealed partial class ProductDetails
{
    private Product? product = null;
    private string message = string.Empty;
    private int currentTypeId = 1;

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        Product? result = await ProductUIService.GetProduct(Id);
        product = result;
        if (product!.Variants.Count > 0)
        {
            currentTypeId = product.Variants[0].ProductTypeId;
        }
    }

    private ProductVariant? GetSelectedVariant()
    {
        return product!.Variants.FirstOrDefault(v => v.ProductTypeId == currentTypeId);
    }

    private async Task AddToCart()
    {
        ProductVariant? productVariant = GetSelectedVariant();
        CartItem cartItem = new()
        {
            ProductId = productVariant!.ProductId,
            ProductTypeId = productVariant.ProductTypeId
        };

        await CartUIService.AddToCart(cartItem);
    }
}
