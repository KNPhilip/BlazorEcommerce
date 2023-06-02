﻿using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Server.Services.CartService
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItemsAsync(List<CartItem> cartItems);
    }
}