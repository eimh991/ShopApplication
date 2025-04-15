using Shop.Model;

namespace Shop.DTO
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImageUrl { get; set; } = string.Empty ;

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
