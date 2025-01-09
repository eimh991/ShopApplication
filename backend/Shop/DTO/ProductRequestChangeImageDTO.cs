namespace Shop.DTO
{
    public class ProductRequestChangeImageDTO
    {
        public int ProductId { get; set; }
        public IFormFile Image { get; set; }
    }
}
