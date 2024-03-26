using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Server.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

public sealed class OrdersController(
    IOrderService orderService) : ControllerTemplate
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public async Task<ActionResult<List<OrderOverviewDto>>> GetOrders() =>
        HandleResult(await _orderService.GetOrders());

    [HttpGet("{orderId}")]
    public async Task<ActionResult<List<OrderOverviewDto>>> GetOrderDetails(int orderId) =>
        HandleResult(await _orderService.GetOrderDetailsAsync(orderId)); 
}
