namespace BlazorEcommerce.Server.Controllers
{
    /// <summary>
    /// Controller Template - Contains the common definition for all controllers
    /// as well as methods for handling common Controller logic. Derives from the ControllerBase class.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerTemplate : ControllerBase
    {
        /// <summary>
        /// Handles a response in the ServiceResponse format and
        /// gives back the appropriate status code, data, error messages, etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns>Status code (depending on the result), and with that either the data,
        /// or a proper error message.</returns>
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