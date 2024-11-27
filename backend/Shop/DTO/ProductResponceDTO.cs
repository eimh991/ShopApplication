namespace Shop.DTO
{
    public class ProductResponceDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }

        public string ImagePath { get; set; } = string.Empty;
    }
}
