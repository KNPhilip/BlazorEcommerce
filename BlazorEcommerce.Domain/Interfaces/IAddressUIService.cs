namespace BlazorEcommerce.Domain.Interfaces
{
    public interface IAddressUIService
    {
        Task<Address> GetAddress();
        Task<Address> AddOrUpdateAddress(Address address);
    }
}
