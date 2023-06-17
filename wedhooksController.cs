using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Webhooks
{
    [Route("api/[controller]")]
    [ApiController]
    public class wedhooksController : ControllerBase
    {
        private readonly ILogger<wedhooksController> _logger;

        public wedhooksController(ILogger<wedhooksController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] dynamic payload)
        {
            // Xử lý logic của bạn dựa trên thông tin nhận được từ webhook

            // Lấy thông tin tin nhắn
            string messageText = payload.message.text;
            string senderId = payload.sender.id;

            _logger.LogInformation($"Received message: {messageText} from sender: {senderId}");

            // Gửi tin nhắn đáp trả
            await SendMessage(senderId, "Xin chào! Cảm ơn bạn đã gửi tin nhắn.");

            return Ok();
        }

        private async Task SendMessage(string recipientId, string messageText)
        {
            // Gửi tin nhắn đến recipientId bằng cách sử dụng API Messenger của Facebook
            // Bạn cần thay thế các thông tin cần thiết và triển khai phương thức này

            // Ví dụ sử dụng thư viện HttpClient để gửi yêu cầu HTTP POST
            using (var client = new HttpClient())
            {
                var requestUri = $"https://graph.facebook.com/v12.0/me/messages?access_token=EAAU9jpyqq6ABAFdXmnQn5GYh2jqH2Pqlb6PgPrr8NGdiZC9ueSId0DwTPIEZCHZBRxwTrNgf3ZAhJZBs9AMNvFU390X0fQY2QJa7y8HE6LdtelFPihbilpnmWLaJeZBjP8gr1ljbPOq5YJpyYFCROK29LswonNQFZBWDDRfShoZCACC4MFNAWh7QfIjVCxgVs2dlb6wOGXjmCwZDZD";

                var messageData = new
                {
                    recipient = new
                    {
                        id = recipientId
                    },
                    message = new
                    {
                        text = messageText
                    }
                };

                var json = JsonConvert.SerializeObject(messageData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Message sent successfully.");
                }
                else
                {
                    _logger.LogError("Failed to send message.");
                }
            }
        }
    }
}
