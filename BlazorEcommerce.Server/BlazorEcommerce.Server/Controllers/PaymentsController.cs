using BlazorEcommerce.Server.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Controllers;

public sealed class PaymentsController(
    IPaymentService paymentService) : ControllerTemplate
{
    private readonly IPaymentService _paymentService = paymentService;

    [HttpPost("checkout"), Authorize]
    public async Task<ActionResult<string>> CreateCheckoutSession()
    {
        try
        {
            Session session = await _paymentService.CreateCheckoutSession();
            return Ok(session.Url);
        }
        catch
        {
            string url = await _paymentService.FakeOrderCompletion();
            return Ok(url);
        }
    }

    [HttpPost]
    public async Task<ActionResult<bool>> FulfillOrder() =>
        HandleResult(await _paymentService.FulfillOrder(Request)); 
}
