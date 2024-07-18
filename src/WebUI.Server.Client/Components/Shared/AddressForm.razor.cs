using Domain.Models;

namespace WebUI.Server.Client.Components.Shared;

public sealed partial class AddressForm
{
    private Address? address = null;
    private bool editAddress = false;

    protected override async Task OnInitializedAsync()
    {
        address = await AddressUIService.GetAddress();
    }

    private async Task SubmitAddresss()
    {
        editAddress = false;
        address = await AddressUIService.AddOrUpdateAddress(address!);
    }

    private void InitAddress()
    {
        address = new();
        editAddress = true;
    }

    private void EditAddress()
    {
        editAddress = true;
    }
}
