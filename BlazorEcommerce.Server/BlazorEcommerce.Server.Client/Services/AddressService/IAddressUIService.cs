using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Client.Services.AddressService
{
    public interface IAddressUIService
    {
        Task<Address> GetAddress();
        Task<Address> AddOrUpdateAddress(Address address);
    }
}
