using Shop.DTO.PaySystemDto;

namespace Shop.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentAsync(PaymentRequestDto requestDto);
        Task<bool> HandleWebhookAsync(WebhookDto webhookData);
    }
}
