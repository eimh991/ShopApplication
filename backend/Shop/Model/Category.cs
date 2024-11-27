namespace Shop.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        // Навигационные свойства
        public List<Product> Products { get; set; } = new List<Product>(); // Связь один ко многим с Product
    }
}
