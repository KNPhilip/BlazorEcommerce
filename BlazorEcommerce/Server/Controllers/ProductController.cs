namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static readonly List<Product> Products = new()
        {
            
        };

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            return Ok(await Task.Run(() => Products));
        }
    }
}