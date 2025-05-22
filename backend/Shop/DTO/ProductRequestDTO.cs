namespace Shop.DTO
{
    public class ProductRequestDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string CategoryTitle { get; set; }

        public IFormFile? Image { get; set; }
    }
}
