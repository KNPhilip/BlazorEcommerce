namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes()
        {
            var response = await _productTypeService.GetProductTypesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType)
        {
            var response = await _productTypeService.AddProductType(productType);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> CreateProductType(ProductType productType)
        {
            var response = await _productTypeService.UpdateProductType(productType);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> DeleteProductType(int productTypeId)
        {
            var response = await _productTypeService.DeleteProductType(productTypeId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}