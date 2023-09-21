namespace BlazorEcommerce.Server.Controllers
{
    [Authorize]
    public class AddressController : ControllerTemplate
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<Address>>> GetAddress() =>
            HandleResult(await _addressService.GetAddress());

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address request) =>
            HandleResult(await _addressService.AddOrUpdateAddress(request));
    }
}