using Shop.DTO.PaySystemDto;
using Shop.Interfaces;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Shop.Extensions;

namespace Shop.Service.PaymentService
{
    public class QiwiPaymentService : IPaymentService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public QiwiPaymentService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["PaymentSetting:QiwiApiKey"]
                ?? throw new ArgumentNullException("Qiwi API key is missing");
            _httpClient = httpClient;
        }
   
        public async Task<PaymentResponseDto> CreatePaymentAsync(PaymentRequestDto requestDto)
        {
            var orderId = Guid.NewGuid().ToString();
            var payload = new
            {
                amount = new { requestDto.Currency, value = requestDto.Amount },
                comment = requestDto.Description,
                successUrl = requestDto.SuccessUrl,
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            // Добавляем API-ключ в заголовки
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // Отправляем запрос в Qiwi API
            var response = await _httpClient.PostAsync($"https://api.qiwi.com/partner/bill/v1/bills/{orderId}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new PaymentException($"Ошибка при создании платежа в Qiwi: {await response.Content.ReadAsStringAsync()}");
            }

            // Формируем URL для оплаты
            string paymentUrl = $"https://oplata.qiwi.com/form/?invoice_uid={orderId}";

            return new PaymentResponseDto
            {
                PaymentUrl = paymentUrl,
                OrderId = orderId,
            };
        }

        public async Task<bool> HandleWebhookAsync(WebhookDto webhookData)
        {
            if(webhookData.Status == "PAID")
            {
                Console.WriteLine($"Платеж {webhookData.OrderId} успешно подтвержден.");
                return true;
            }
            return false;
        }
    }
}
