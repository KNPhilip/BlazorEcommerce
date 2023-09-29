namespace BlazorEcommerce.Server.Services.AddressService
{
    /// <summary>
    /// Implementation class of IAddressService.
    /// </summary>
    public class AddressService : IAddressService
    {
        #region Fields
        /// <summary>
        /// EcommerceContext field. Used to access the database context.
        /// </summary>
        private readonly EcommerceContext _context;
        /// <summary>
        /// IAuthService field. Used to access the Auth Services.
        /// </summary>
        private readonly IAuthService _authService; 
        #endregion

        #region Constructor
        /// <param name="context">EcommerceContext instance to be passed on to the correct
        /// field, containing the correct implementation through the IoC container.</param>
        /// <param name="authService">IAuthService instance to be passed on to the correct
        /// field, containing the correct implementation class through the IoC container.</param>
        public AddressService(EcommerceContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Recieves the address of the currently authenticated user.
        /// </summary>
        /// <returns>Address object.</returns>
        public async Task<ServiceResponse<Address>> GetAddress()
        {
            int userId = _authService.GetNameIdFromClaims();
            Address? address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId);

            return address is not null
                ? ServiceResponse<Address>.SuccessResponse(address)
                : new ServiceResponse<Address> { Error = "No address found." };
        }

        /// <summary>
        /// Adds or updates the address of the currently authenticated user.
        /// </summary>
        /// <param name="address">Represents the given address to add or update.</param>
        /// <returns>The added or updated address.</returns>
        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
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

            return ServiceResponse<Address>.SuccessResponse(response);
        } 
        #endregion
    }
}