using Shop.Model;

namespace Shop.DTO
{
    public class CardItemResponseDTO
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }

    }
}
