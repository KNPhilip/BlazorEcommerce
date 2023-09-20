namespace BlazorEcommerce.Server.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly EcommerceContext _context;
        private readonly IAuthService _authService;

        public AddressService(EcommerceContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
        {
            Address response;
            var dbAddress = (await GetAddress()).Data;
            if (dbAddress is null)
            {
                address.UserId = _authService.GetNameIdFromClaims();
                _context.Addresses.Add(address);
                response = address;
            }
            else
            {
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

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            int userId = _authService.GetNameIdFromClaims();
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId);

            return address is not null 
                ? ServiceResponse<Address>.SuccessResponse(address)
                : new ServiceResponse<Address> { Error = "No address found." };
        }
    }
}