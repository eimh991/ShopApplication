namespace Shop.DTO.PaySystemDto
{
    public class QiwiWebHookDto
    {
        public string BillId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }

    }
}
