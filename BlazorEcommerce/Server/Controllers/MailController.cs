﻿namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        public IAuthService _authService;
        public IMailService _mailService;

        public MailController(IMailService mailService, IAuthService authService)
        {
            _authService = authService;
            _mailService = mailService;
        }

        [HttpPost("send")]
        public async Task<ActionResult<ServiceResponse<bool>>> SendEmail(SendMailDto request)
        {
            var response = await _mailService.SendEmailAsync(request.ToEmail, request.Subject, request.HTMLBody);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}