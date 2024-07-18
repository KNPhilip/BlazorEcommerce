using Domain.Dtos;
using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Client.Components.Pages;

public sealed partial class Cart
{
    List<CartProductResponseDto>? cartProducts = null;
    string message = "Loading cart...";
    bool isAuthenticated = false;

    protected override async Task OnInitializedAsync()
    {
        isAuthenticated = await AuthUIService.IsUserAuthenticated();
        await LoadCart();
    }

    private async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        await CartUIService.RemoveProductFromCart(productId, productTypeId);
        await LoadCart();
    }

    private async Task LoadCart()
    {
        await CartUIService.GetCartItemsCount();
        cartProducts = await CartUIService.GetCartProducts();
        if (cartProducts is null || cartProducts.Count == 0)
        {
            message = "Your cart is empty.";
        }
    }

    private async Task UpdateQuantity(ChangeEventArgs e, CartProductResponseDto product)
    {
        product.Quantity = int.Parse(e.Value!.ToString()!);
        if (product.Quantity < 1)
        {
            product.Quantity = 1;
        }
        await CartUIService.UpdateQuantity(product);
    }

    private async Task PlaceOrder()
    {
        string url = await OrderUIService.PlaceOrder();
        NavigationManager.NavigateTo(url);
    }
}
