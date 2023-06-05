namespace BlazorEcommerce.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _http;

        public ProductTypeService(HttpClient http)
        {
            _http = http;
        }
    }
}