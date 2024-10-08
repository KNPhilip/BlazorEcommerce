﻿using Domain.Dtos;
using Domain.Models;

namespace WebUI.Server.Services.CartService;

public interface ICartService
{
    Task<ResponseDto<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems);
    Task<ResponseDto<List<CartProductResponseDto>>> StoreCartItemsAsync(List<CartItem> cartItems);
    Task<ResponseDto<int>> GetCartItemsCountAsync();
    Task<ResponseDto<List<CartProductResponseDto>>> GetDbCartItems(string? userId = null);
    Task<ResponseDto<bool>> AddToCart(CartItem cartItem);
    Task<ResponseDto<bool>> UpdateQuantity(CartItem cartItem);
    Task<ResponseDto<bool>> RemoveItemFromCart(int productId, int productTypeId);
}
