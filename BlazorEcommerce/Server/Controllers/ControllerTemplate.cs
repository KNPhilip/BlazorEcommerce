namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerTemplate : ControllerBase
    {
        protected ActionResult HandleResult<T>(ServiceResponse<T> response)
        {
            return response.Success
                ? response.Data is null
                    ? NotFound()
                    : Ok(response.Data)
                : BadRequest(response.Error);
        }
    }
}