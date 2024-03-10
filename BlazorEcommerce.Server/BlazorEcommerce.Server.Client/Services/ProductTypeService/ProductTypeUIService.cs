using BlazorEcommerce.Domain.Models;
using System.Net.Http.Json;

namespace BlazorEcommerce.Server.Client.Services.ProductTypeService
{
    public class ProductTypeUIService : IProductTypeUIService
    {
        private readonly HttpClient _http;

        public ProductTypeUIService(HttpClient http)
        {
            _http = http;
        }

        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public event Action? OnChange;

        public async Task AddProductType(ProductType productType)
        {
            productType.Editing = productType.IsNew = false;
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/v1/producttypes", productType);
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
            List<ProductType> result = await _http
                .GetFromJsonAsync<List<ProductType>>("api/v1/producttypes") ?? [];
            ProductTypes = result;
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response = await _http.PutAsJsonAsync("api/v1/producttypes", productType);
            ProductTypes = await response.Content
                .ReadFromJsonAsync<List<ProductType>>() ?? [];
            OnChange!.Invoke();
        }

        public async Task DeleteProductType(int productTypeId)
        {
            var response = await _http.DeleteAsync($"api/v1/producttypes/{productTypeId}");
            ProductTypes = await response.Content
                .ReadFromJsonAsync<List<ProductType>>() ?? [];
            OnChange!.Invoke();
        }
    }
}
