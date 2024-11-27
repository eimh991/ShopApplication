namespace Shop.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string CategoryTitle { get; set; }

        public string ImagePath { get; set; } = string.Empty;
    }
}
