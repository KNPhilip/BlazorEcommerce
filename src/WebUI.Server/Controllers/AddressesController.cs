using Domain.Models;
using WebUI.Server.Services.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Server.Controllers;

[Authorize]
public sealed class AddressesController(
    IAddressService addressService) : ControllerTemplate
{
    private readonly IAddressService _addressService = addressService;

    [HttpGet]
    public async Task<ActionResult<Address>> GetAddress() =>
        HandleResult(await _addressService.GetAddress());

    [HttpPost]
    public async Task<ActionResult<Address>> AddOrUpdateAddress(Address request) =>
        HandleResult(await _addressService.AddOrUpdateAddress(request)); 
}
