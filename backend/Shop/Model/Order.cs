namespace Shop.Model
{
    
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Внешний ключ и связь
        public int UserId { get; set; }
        public User? User { get; set; } // Связь многие к одному с User

        // Навигационные свойства
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Связь один ко многим с OrderItem
    }
}
