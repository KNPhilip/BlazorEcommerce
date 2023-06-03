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

        public Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            int userId = _authService.GetNameIdFromClaims();
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId);
            return new ServiceResponse<Address>
            {
                Data = address
            };
        }
    }
}