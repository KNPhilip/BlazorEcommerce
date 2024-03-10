using BlazorEcommerce.Domain.Models;
using System.Net.Http.Json;

namespace BlazorEcommerce.Server.Client.Services.AddressService
{
    public class AddressUIService : IAddressUIService
    {
        private readonly HttpClient _http;

        public AddressUIService(HttpClient http)
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
