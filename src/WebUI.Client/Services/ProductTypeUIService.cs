using Domain.Interfaces;
using Domain.Models;
using System.Net.Http.Json;

namespace WebUI.Client.Services;

public sealed class ProductTypeUIService(HttpClient http) : IProductTypeUIService
{
    public List<ProductType> ProductTypes { get; set; } = [];

    public event Action? OnChange;

    public async Task AddProductType(ProductType productType)
    {
        productType.Editing = productType.IsNew = false;
        HttpResponseMessage response = await http.PostAsJsonAsync("api/v1/producttypes", productType);
        ProductTypes = (await response.Content
            .ReadFromJsonAsync<List<ProductType>>())!;
        OnChange!.Invoke();
    }

    public ProductType CreateNewProductType()
    {
        ProductType newProductType = new()
        {
            IsNew = true,
            Editing = true,
        };

        ProductTypes.Add(newProductType);
        OnChange!.Invoke();
        return newProductType;
    }

    public async Task GetProductTypes()
    {
        List<ProductType> result = await http
            .GetFromJsonAsync<List<ProductType>>("api/v1/producttypes") ?? [];
        ProductTypes = result;
    }

    public async Task UpdateProductType(ProductType productType)
    {
        HttpResponseMessage response = await http
            .PutAsJsonAsync("api/v1/producttypes", productType);
        ProductTypes = await response.Content
            .ReadFromJsonAsync<List<ProductType>>() ?? [];
        OnChange!.Invoke();
    }

    public async Task DeleteProductType(int productTypeId)
    {
        HttpResponseMessage response = await http
            .DeleteAsync($"api/v1/producttypes/{productTypeId}");
        ProductTypes = await response.Content
            .ReadFromJsonAsync<List<ProductType>>() ?? [];
        OnChange!.Invoke();
    }
}
