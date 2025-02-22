namespace Shop.DTO.PaySystemDto
{
    public class PaymentRequestDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RUB";
        public string Description { get; set; }
        public string SuccessUrl { get; set; }
    }
}
