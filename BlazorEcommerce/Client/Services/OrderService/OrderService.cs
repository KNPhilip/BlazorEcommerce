namespace BlazorEcommerce.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(
            HttpClient http,
            AuthenticationStateProvider authStateProvider,
            NavigationManager navigationManager )
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            ServiceResponse<OrderDetailsDto>? result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsDto>>($"api/v1/orders/{orderId}");
            return result!.Data!;
        }

        public async Task<List<OrderOverviewDto>> GetOrders()
        {
            ServiceResponse<List<OrderOverviewDto>>? result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewDto>>>("api/v1/orders");
            return result!.Data!;
        }

        public async Task<string> PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _http.PostAsync("api/v1/payments/checkout", null);
                return await result.Content.ReadAsStringAsync();
            }
            else return "login";
        }

        private async Task<bool> IsUserAuthenticated() =>
            (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
    }
}