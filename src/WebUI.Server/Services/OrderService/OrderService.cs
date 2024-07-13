using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.AuthService;
using BlazorEcommerce.Server.Services.CartService;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.OrderService;

public sealed class OrderService(
    EcommerceContext context,
    ICartService cartService,
    IAuthService authService) : IOrderService
{
    private readonly EcommerceContext _context = context;
    private readonly ICartService _cartService = cartService;
    private readonly IAuthService _authService = authService;

    public async Task<ResponseDto<bool>> PlaceOrder(int userId)
    {
        List<CartProductResponseDto>? products = (await _cartService.GetDbCartItems(userId)).Data;
        decimal totalPrice = 0;
        products?.ForEach(product => totalPrice += product.Price * product.Quantity);

        Order order = new()
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            TotalPrice = totalPrice
        };

        products?.ForEach(product => order.OrderItems.Add(new OrderItem
        {
            Order = order,
            ProductId = product.ProductId,
            ProductTypeId = product.ProductTypeId,
            Quantity = product.Quantity,
            TotalPrice = product.Price * product.Quantity
        }));

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(_context.CartItems
            .Where(ci => ci.UserId == userId));

        await _context.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }

    public async Task<ResponseDto<OrderDetailsDto>> GetOrderDetailsAsync(int orderId)
    {
        Order? order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p!.Images)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductType)
            .Where(o => o.UserId == _authService
                .GetNameIdFromClaims() && o.Id == orderId)
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefaultAsync();

        if (order is null)
        {
            return ResponseDto<OrderDetailsDto>.ErrorResponse("Order not found.");
        }

        OrderDetailsDto orderDetailsResponse = new()
        {
            OrderDate = order.OrderDate,
            TotalPrice = order.TotalPrice,
            Products = []
        };

        order.OrderItems.ForEach(item =>
        orderDetailsResponse.Products.Add(new OrderDetailsProductDto
        {
            // TODO Might be wise to add Automapper / Mapster here
            ProductId = item.ProductId,
            ImageUrl = item.Product!.ImageUrl,
            Images = item.Product.Images,
            ProductType = item.ProductType!.Name,
            Quantity = item.Quantity,
            Title = item.Product.Title,
            TotalPrice = item.TotalPrice
        }));

        return ResponseDto<OrderDetailsDto>.SuccessResponse(orderDetailsResponse);
    }

    public async Task<ResponseDto<List<OrderOverviewDto>>> GetOrders()
    {
        List<Order> orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p!.Images)
            .Where(o => o.UserId == _authService.GetNameIdFromClaims())
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        List<OrderOverviewDto> orderResponse = [];

        orders.ForEach(o => orderResponse.Add(new OrderOverviewDto
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            TotalPrice = o.TotalPrice,
            Product = o.OrderItems.Count > 1 ?
                $"{o.OrderItems.First().Product!.Title} and " +
                $"{o.OrderItems.Count - 1} more..." :
                o.OrderItems.First().Product!.Title,
            ProductImageUrl = o.OrderItems.First().Product!.ImageUrl,
            Images = o.OrderItems.First().Product!.Images
        }));

        return ResponseDto<List<OrderOverviewDto>>.SuccessResponse(orderResponse);
    }
}
