using Microsoft.AspNetCore.Mvc;
using Shop.DTO.PaySystemDto;
using Shop.Extensions;
using Shop.Interfaces;
using Shop.Service.PaymentService;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IDictionary<string, IPaymentService> _paymentServices;

        public PaymentController(QiwiPaymentService qiwiService, TinkoffPaymentService tinkoffService)
        {
            _paymentServices = new Dictionary<string, IPaymentService>
            {
                { "qiwi", qiwiService },
                { "tinkoff", tinkoffService }
            };
        }

        [HttpPost("{provider}")]
        public async Task<IActionResult> CreatePayment(string provider, [FromBody] PaymentRequestDto request)
        {
            if (_paymentServices.TryGetValue(provider.ToLower(), out var paymentService))
            {
                return BadRequest(new { error = "Неверный платежный провайдер" });
            }
            try
            {
                var paymentResponse = await paymentService.CreatePaymentAsync(request);
                return Ok(paymentResponse);
            }
            catch (PaymentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("webhook/{provider}")]
        public async Task<IActionResult> HandleWebhook(string provider, [FromBody] WebhookDto webhookData)
        {
            if (!_paymentServices.TryGetValue(provider.ToLower(), out var paymentService))
            {
                return BadRequest(new { error = "Не верный платежный провайдер" });
            }
            try
            {
                bool success = await paymentService.HandleWebhookAsync(webhookData);
                return success
                    ? Ok(new { message = "Платеж подтвержден" })
                    : BadRequest(new { error = "Ошибка обработки вебхука" });
            }
            catch (PaymentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
    }
}
