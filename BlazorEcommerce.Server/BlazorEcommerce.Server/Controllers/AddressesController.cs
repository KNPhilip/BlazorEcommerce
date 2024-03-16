using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Services.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Authorize]
    public sealed class AddressesController : ControllerTemplate
    {
        private readonly IAddressService _addressService; 

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<Address>> GetAddress() =>
            HandleResult(await _addressService.GetAddress());

        [HttpPost]
        public async Task<ActionResult<Address>> AddOrUpdateAddress(Address request) =>
            HandleResult(await _addressService.AddOrUpdateAddress(request)); 
    }
}
