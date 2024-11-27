namespace Shop.Model
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Внешние ключи и связи
        public int OrderId { get; set; }
        public Order? Order { get; set; } // Связь многие к одному с Order

        public int ProductId { get; set; }
        public Product? Product { get; set; } // Связь многие к одному с Product
    }
}
