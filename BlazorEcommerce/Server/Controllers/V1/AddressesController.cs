namespace BlazorEcommerce.Server.Controllers.V1
{
    /// <summary>
    /// Address Controller - Contains all endpoints regarding addresses.
    /// </summary>
    [Authorize]
    public class AddressesController : ControllerTemplate
    {
        #region Fields
        /// <summary>
        /// IAddressService field. Used to access the Address Services.
        /// </summary>
        private readonly IAddressService _addressService; 
        #endregion

        #region Constructor
        /// <param name="addressService">IAddressService instance to be passed on to the
        /// field, containing the correct implementation class through the IoC container.</param>
        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        } 
        #endregion

        #region Endpoints
        /// <summary>
        /// Endpoint to recieve the address of the authenticated user.
        /// </summary>
        /// <returns>The appropriate address data or an error in case of the user not having added an address.</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<Address>>> GetAddress() =>
            HandleResult(await _addressService.GetAddress());

        /// <summary>
        /// Endpoint to create or update the currently authenticated users address.
        /// </summary>
        /// <param name="request">Represents the given Address to be added or updated.</param>
        /// <returns>The updated address data or an error in case of failure.</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address request) =>
            HandleResult(await _addressService.AddOrUpdateAddress(request)); 
        #endregion
    }
}