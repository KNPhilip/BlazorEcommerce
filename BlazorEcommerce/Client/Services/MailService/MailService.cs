namespace BlazorEcommerce.Client.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly HttpClient _http;

        public MailService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<bool>> SendEmail(SendMail request)
        {
            var res = await _http.PostAsJsonAsync("api/mail/send", request);
            return await res.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}