using Microsoft.Extensions.Options;
using System.Text;

namespace HKQTravellingAuthenication.Services
{
    public class SpeedSMSSettings
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }

    public class SendSmsService : ISmsSender
    {
        private readonly SpeedSMSSettings _speedSmsSettings;

        private readonly HttpClient _httpClient;

        private readonly ILogger<SendSmsService> _logger;

        public SendSmsService(IOptions<SpeedSMSSettings> speedSmsSettings, IHttpClientFactory httpClientFactory, ILogger<SendSmsService> logger)
        {
            _speedSmsSettings = speedSmsSettings.Value;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _logger.LogInformation("Create SendSmsService");
        }

        //public Task SendSmsAsync(string number, string message)
        //{
        //    Cài đặt dịch vụ gửi SMS tại đây
        //    System.IO.Directory.CreateDirectory("smssave");
        //    var emailsavefile = string.Format(@"smssave/{0}-{1}.txt", number, Guid.NewGuid());
        //    System.IO.File.WriteAllTextAsync(emailsavefile, message);
        //    return Task.FromResult(0);
        //}
        public async Task SendSmsAsync(string number, string message)
        {
            var requestData = new
            {
                to = number,
                content = message
            };

            var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", _speedSmsSettings.ApiKey);

            try
            {
                var response = await _httpClient.PostAsync(_speedSmsSettings.ApiUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"SMS đã gửi thành công đến {number}");
                }
                else
                {
                    _logger.LogError($"Lỗi gửi SMS đến {number}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while sending SMS to {number}: {ex.Message}");
            }
        }
    }
}
