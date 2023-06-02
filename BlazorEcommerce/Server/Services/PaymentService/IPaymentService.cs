namespace BlazorEcommerce.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FulfillOrdder(HttpRequest request);
    }
}