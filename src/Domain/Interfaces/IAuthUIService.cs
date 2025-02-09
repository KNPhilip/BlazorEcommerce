namespace Domain.Interfaces;

public interface IAuthUIService
{
    Task<bool> IsUserAuthenticated();
}
