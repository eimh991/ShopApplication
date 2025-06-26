namespace Shop.Model
{
    
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } // Связь многие к одному с User
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Связь один ко многим с OrderItem
    }
}
