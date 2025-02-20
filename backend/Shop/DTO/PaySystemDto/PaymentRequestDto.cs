namespace Shop.DTO.PaySystemDto
{
    public class PaymentRequestDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string SuccessUrl { get; set; }
    }
}
