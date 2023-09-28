namespace BlazorEcommerce.Server.Controllers.V1
{
    /// <summary>
    /// Product Type Controller - Contains all endpoints regarding Product Types. This is meant for administrators only.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ProductTypesController : ControllerTemplate
    {
        /// <summary>
        /// IProductTypeService field. Used to access the Product Type Services.
        /// </summary>
        private readonly IProductTypeService _productTypeService;

        /// <param name="productTypeService">IProductTypeService instance to be passed on to the
        /// field, containing the correct implementation class through the IoC container.</param>
        public ProductTypesController(IProductTypeService productTypeService)
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
        /// <param name="productType">Represents the given product type to be added to the database.</param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType) =>
            HandleResult(await _productTypeService.AddProductType(productType));

        /// <summary>
        /// Endpoint to update the Product Type from the database.
        /// </summary>
        /// <param name="productType">Represents the given product type
        /// to be updated in the database (ID must be included)</param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> UpdateProductType(ProductType productType) =>
            HandleResult(await _productTypeService.UpdateProductType(productType));

        /// <summary>
        /// Endpoint to delete the Product Type from the database with the given ID.
        /// </summary>
        /// <param name="productTypeId">Represents the ID of the product type to delete from the database.</param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpDelete("{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> DeleteProductType(int productTypeId) =>
            HandleResult(await _productTypeService.DeleteProductType(productTypeId));
    }
}