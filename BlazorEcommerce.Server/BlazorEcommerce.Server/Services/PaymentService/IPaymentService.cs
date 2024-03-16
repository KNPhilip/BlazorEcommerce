using BlazorEcommerce.Domain.Dtos;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<string> FakeOrderCompletion();
        Task<Session> CreateCheckoutSession();
        Task<ResponseDto<bool>> FulfillOrder(HttpRequest request);
    }
}
