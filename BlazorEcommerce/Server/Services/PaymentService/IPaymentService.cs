namespace BlazorEcommerce.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<string> FakeOrderCompletion();
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
    }
}