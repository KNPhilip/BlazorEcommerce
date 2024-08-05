using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Data;
using WebUI.Server.Services.AuthService;
using WebUI.Server.Services.CartService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace WebUI.Server.Services.OrderService;

public sealed class OrderService(
    IDbContextFactory<EcommerceContext> contextFactory,
    ICartService cartService,
    IAuthService authService) : IOrderService
{
    private readonly IDbContextFactory<EcommerceContext> _contextFactory = contextFactory;
    private readonly ICartService _cartService = cartService;
    private readonly IAuthService _authService = authService;

    public async Task<ResponseDto<bool>> PlaceOrder(string userId)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

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

        dbContext.Orders.Add(order);
        dbContext.CartItems.RemoveRange(dbContext.CartItems
            .Where(ci => ci.UserId == userId));

        await dbContext.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }

    public async Task<ResponseDto<OrderDetailsDto>> GetOrderDetailsAsync(int orderId)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        string userId = await _authService.GetUserIdAsync();

        Order? order = await dbContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p!.Images)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductType)
            .Where(o => o.UserId == userId && o.Id == orderId)
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
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        string userId = await _authService.GetUserIdAsync();

        List<Order> orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p!.Images)
            .Where(o => o.UserId == userId)
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
