namespace Shop.DTO.PaySystemDto
{
    public class TinkoffWebhookDto
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}
