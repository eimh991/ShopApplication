using Shop.DTO.PaySystemDto;

namespace Shop.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentAsync(PaymentRequestDto requestDto, CancellationToken cancellationToken);
        Task<bool> HandleWebhookAsync(WebhookDto webhookData , CancellationToken cancellationToken);
    }
}
