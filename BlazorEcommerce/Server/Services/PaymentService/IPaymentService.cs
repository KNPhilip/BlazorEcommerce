namespace BlazorEcommerce.Server.Services.PaymentService
{
    /// <summary>
    /// Interface for all things regarding Payment Services.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Fake completes the order to simulate a complete purchase.
        /// </summary>
        /// <returns>A string containing the URL to go to next.</returns>
        Task<string> FakeOrderCompletion();

        /// <summary>
        /// Creates a new Stripe checkout session.
        /// </summary>
        /// <returns>The Stripe session.</returns>
        Task<Session> CreateCheckoutSession();

        /// <summary>
        /// Completes the Stripe order.
        /// </summary>
        /// <returns>True/False depending on the success of the order fulfillment.</returns>
        Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
    }
}