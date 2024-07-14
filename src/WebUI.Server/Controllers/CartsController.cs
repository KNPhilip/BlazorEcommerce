using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Services.CartService;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Server.Controllers;

public sealed class CartsController(
    ICartService cartService) : ControllerTemplate
{
    private readonly ICartService _cartService = cartService;

    [HttpPost("products")]
    public async Task<ActionResult<List<CartProductResponseDto>>> GetCartProducts(List<CartItem> cartItems) =>
        HandleResult(await _cartService.GetCartProductsAsync(cartItems));

    [HttpPost]
    public async Task<ActionResult<List<CartProductResponseDto>>> StoreCartItems(List<CartItem> cartItems) =>
        HandleResult(await _cartService.StoreCartItemsAsync(cartItems));

    [HttpPost("add")]
    public async Task<ActionResult<bool>> AddToCart(CartItem cartItem) =>
        HandleResult(await _cartService.AddToCart(cartItem));

    [HttpPut("update-quantity")]
    public async Task<ActionResult<bool>> UpdateQuantity(CartItem cartItem) =>
        HandleResult(await _cartService.UpdateQuantity(cartItem));

    [HttpDelete("{productId}/{productTypeId}")]
    public async Task<ActionResult<bool>> RemoveItemFromCart(int productId, int productTypeId) =>
        HandleResult(await _cartService.RemoveItemFromCart(productId, productTypeId));

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCartItemsCount() =>
        HandleResult(await _cartService.GetCartItemsCountAsync());

    [HttpGet]
    public async Task<ActionResult<List<CartProductResponseDto>>> GetDbCartItems() =>
        HandleResult(await _cartService.GetDbCartItems()); 
}
