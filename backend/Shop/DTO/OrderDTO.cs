namespace Shop.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemDTO> orderItemDTOs { get; set; } = new List<OrderItemDTO>();
    }
}
