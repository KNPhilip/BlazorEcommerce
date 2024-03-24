using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Interfaces;
using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Services.AddressService;

public sealed class AddressUIService(
    IAddressService addressService) : IAddressUIService
{
    public async Task<Address> AddOrUpdateAddress(Address address)
    {
        ResponseDto<Address> result = await addressService
            .AddOrUpdateAddress(address);
        return result.Data!;
    }

    public async Task<Address> GetAddress()
    {
        ResponseDto<Address> result = await addressService.GetAddress();
        return result.Data!;
    }
}
