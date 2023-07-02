namespace BlazorEcommerce.Client.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly HttpClient _http;

        public MailService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> SendEmail(SendMailDto request)
        {
            var response = await _http.PostAsJsonAsync("api/mail/send", request);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}