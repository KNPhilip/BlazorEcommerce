namespace BlazorEcommerce.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : ControllerTemplate
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes() =>
            HandleResult(await _productTypeService.GetProductTypesAsync());

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType) =>
            HandleResult(await _productTypeService.AddProductType(productType));

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> CreateProductType(ProductType productType) =>
            HandleResult(await _productTypeService.UpdateProductType(productType));

        [HttpDelete("{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> DeleteProductType(int productTypeId) =>
            HandleResult(await _productTypeService.DeleteProductType(productTypeId));
    }
}