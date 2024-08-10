namespace Domain.Interfaces;

public interface IProductTypeUIService
{
    event Action OnChange;
    public List<ProductType> ProductTypes { get; set; }
    Task GetProductTypes();
    Task AddProductType(ProductType productType);
    Task UpdateProductType(ProductType productType);
    Task DeleteProductType(int productTypeId);
    ProductType CreateNewProductType();
}
