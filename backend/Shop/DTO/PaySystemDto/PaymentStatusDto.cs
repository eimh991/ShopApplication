namespace Shop.DTO.PaySystemDto
{
    public class PaymentStatusDto
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
