using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.AuthService;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.AddressService;

public sealed class AddressService(EcommerceContext context, 
    IAuthService authService) : IAddressService
{
    private readonly EcommerceContext _context = context;
    private readonly IAuthService _authService = authService;

    public async Task<ResponseDto<Address>> GetAddress()
    {
        int userId = _authService.GetNameIdFromClaims();
        Address? address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId);

        return address is not null
            ? ResponseDto<Address>.SuccessResponse(address)
            : ResponseDto<Address>.ErrorResponse("No address found.");
    }

    public async Task<ResponseDto<Address>> AddOrUpdateAddress(Address address)
    {
        Address response;
        Address? dbAddress = (await GetAddress()).Data;
        if (dbAddress is null)
        {
            address.UserId = _authService.GetNameIdFromClaims();
            _context.Addresses.Add(address);
            response = address;
        }
        else
        {
            // TODO Might be wise to introduce AutoMapper / Mapster here
            dbAddress.FirstName = address.FirstName;
            dbAddress.LastName = address.LastName;
            dbAddress.Street = address.Street;
            dbAddress.Zip = address.Zip;
            dbAddress.City = address.City;
            dbAddress.State = address.State;
            dbAddress.Country = address.Country;
            response = dbAddress;
        }

        await _context.SaveChangesAsync();

        return ResponseDto<Address>.SuccessResponse(response);
    } 
}
