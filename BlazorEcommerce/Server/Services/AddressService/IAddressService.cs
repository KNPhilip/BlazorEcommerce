namespace BlazorEcommerce.Server.Services.AddressService
{
    /// <summary>
    /// Interface for all things regarding Address Services.
    /// </summary>
    public interface IAddressService
    {
        /// <summary>
        /// Recieves the address of the currently authenticated user.
        /// </summary>
        /// <returns>Address object.</returns>
        Task<ServiceResponse<Address>> GetAddress();

        /// <summary>
        /// Adds or updates the address of the currently authenticated user.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>The added or updated address.</returns>
        Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
    }
}