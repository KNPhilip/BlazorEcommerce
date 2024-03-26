using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Services.AddressService;

public interface IAddressService
{
    Task<ResponseDto<Address>> GetAddress();
    Task<ResponseDto<Address>> AddOrUpdateAddress(Address address);
}
