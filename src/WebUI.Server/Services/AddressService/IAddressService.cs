using Domain.Dtos;
using Domain.Models;

namespace WebUI.Server.Services.AddressService;

public interface IAddressService
{
    Task<ResponseDto<Address>> GetAddress();
    Task<ResponseDto<Address>> AddOrUpdateAddress(Address address);
}
