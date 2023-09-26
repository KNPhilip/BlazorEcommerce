namespace BlazorEcommerce.Server.Controllers
{
    /// <summary>
    /// Product Type Controller - Contains all endpoints regarding Product Types. This is meant for administrators only.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : ControllerTemplate
    {
        /// <summary>
        /// IProductTypeService instance. This accesses the implementation class of the ProductTypeService through the IoC container.
        /// </summary>
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        /// <summary>
        /// Endpoint to get all of the different Product Types.
        /// </summary>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes() =>
            HandleResult(await _productTypeService.GetProductTypesAsync());

        /// <summary>
        /// Endpoint to create a new Product Type.
        /// </summary>
        /// <param name="productType"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType) =>
            HandleResult(await _productTypeService.AddProductType(productType));

        /// <summary>
        /// Endpoint to update the Product Type from the database.
        /// </summary>
        /// <param name="productType"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> CreateProductType(ProductType productType) =>
            HandleResult(await _productTypeService.UpdateProductType(productType));

        /// <summary>
        /// Endpoint to delete the Product Type from the database with the given ID.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpDelete("{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> DeleteProductType(int productTypeId) =>
            HandleResult(await _productTypeService.DeleteProductType(productTypeId));
    }
}