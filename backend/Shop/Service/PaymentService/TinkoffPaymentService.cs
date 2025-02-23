using Shop.DTO.PaySystemDto;
using Shop.Extensions;
using Shop.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Shop.Service.PaymentService
{
    public class TinkoffPaymentService : IPaymentService
    {
        private readonly string _apiKey;
        private readonly string _terminalKey;
        private readonly HttpClient _httpClient;

        public TinkoffPaymentService(IConfiguration configuration, HttpClient client)
        {
            _apiKey = configuration["PaymentSettings:TinkoffApiKey"] ??
                throw new PaymentException("Tinkoff API key is missing");
            _terminalKey = configuration["PaymentSettings:TinkoffterminalKey"] ??
                throw new PaymentException("Tinkoff Terminal key is missing");
            _httpClient = client;
        }
        public async Task<PaymentResponseDto> CreatePaymentAsync(PaymentRequestDto requestDto)
        {
            var orderId =  Guid.NewGuid().ToString();

            var payload = new
            {
                TerminalKey = _terminalKey,
                Amount = requestDto.Amount * 100,
                OrderId = orderId,
                Description = requestDto.Description,
                SuccessUrl = requestDto.SuccessUrl,
            };

            var jsonPayload =  JsonSerializer.Serialize(payload);
            var content =  new StringContent(jsonPayload, Encoding.UTF8, "applecation/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://securepay.tinkoff.ru/v2/Init", content);

            if (!response.IsSuccessStatusCode){
                throw new PaymentException($"Ошибка при создании платежа в Tinkoff: {await response.Content.ReadAsStringAsync()}");
            }

            var responseContext = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<PaymentResponseDto>(responseContext);

            if(responseData == null ){
                throw new PaymentException("Ошибка API Tinkoff");
            }
            return responseData;
        }

        public async Task<bool> HandleWebhookAsync(WebhookDto webhookData)
        {
            Console.WriteLine($"Tinkoff Webhook: OrderId={webhookData.OrderId}, Status={webhookData.Status}");
            //если статус = "CONFIRMED" то платеж совершен и подтвержденн.
            if (webhookData.Status.ToLower() == "CONFIRMED")
            {
                Console.WriteLine($"Платеж для OrderId {webhookData.OrderId} был подтвержден.");
                return true;  // Успешно обработан
            }
            Console.WriteLine($"Платеж для OrderId {webhookData.OrderId} не подтвержден.");
            return false;  // Неуспешно обработан
        }
    }
}
