namespace Shop.DTO
{
    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int UserId { get; set; }

        public int Quantity { get; set; }
        public int CategoryId { get; set; }

        public string ImagePath { get; set; } = "default.jpg";
    }
}
