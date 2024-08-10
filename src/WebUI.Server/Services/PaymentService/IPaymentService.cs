using Domain.Dtos;
using Stripe.Checkout;

namespace WebUI.Server.Services.PaymentService;

public interface IPaymentService
{
    Task<string> FakeOrderCompletion();
    Task<Session> CreateCheckoutSession();
    Task<ResponseDto<bool>> FulfillOrder(HttpRequest request);
}
