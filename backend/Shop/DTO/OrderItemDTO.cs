using Shop.Model;

namespace Shop.DTO
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
