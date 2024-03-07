namespace BlazorEcommerce.Client.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _http;

        public AddressService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Address> AddOrUpdateAddress(Address address)
        {
            var response = await _http.PostAsJsonAsync("api/v1/addresses", address);
            return response.Content
                .ReadFromJsonAsync<Address>().Result!;
        }

        public async Task<Address> GetAddress()
        {
            Address? response = await _http.GetFromJsonAsync<Address>("api/v1/addresses");
            return response!;
        }
    }
}