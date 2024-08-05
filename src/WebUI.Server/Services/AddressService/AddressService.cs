using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Data;
using WebUI.Server.Services.AuthService;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Server.Services.AddressService;

public sealed class AddressService(IDbContextFactory<EcommerceContext> contextFactory, 
    IAuthService authService) : IAddressService
{
    private readonly IDbContextFactory<EcommerceContext> _contextFactory = contextFactory;
    private readonly IAuthService _authService = authService;

    public async Task<ResponseDto<Address>> GetAddress()
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        string userId = await _authService.GetUserIdAsync();
        
        Address? address = await dbContext.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId);

        return address is not null
            ? ResponseDto<Address>.SuccessResponse(address)
            : ResponseDto<Address>.ErrorResponse("No address found.");
    }

    public async Task<ResponseDto<Address>> AddOrUpdateAddress(Address address)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        Address response;
        Address? dbAddress = (await GetAddress()).Data;
        if (dbAddress is null)
        {
            address.UserId = await _authService.GetUserIdAsync();
            dbContext.Addresses.Add(address);
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

        await dbContext.SaveChangesAsync();

        return ResponseDto<Address>.SuccessResponse(response);
    } 
}
