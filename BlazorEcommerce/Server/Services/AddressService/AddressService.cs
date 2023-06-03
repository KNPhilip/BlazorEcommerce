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
    }
}