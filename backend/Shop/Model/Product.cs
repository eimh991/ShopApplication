namespace Shop.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        // Внешний ключ и связь
        public int CategoryId { get; set; }
        public Category? Category { get; set; } // Связь многие к одному с Category

        // Навигационные свойства
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Связь один ко многим с OrderItem
        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Связь один ко многим с CartItem
    }
}
