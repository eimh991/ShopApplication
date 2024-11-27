namespace Shop.Model
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }

        public bool IsVisible { get; set; } = true; // Видимость товара в корзине

        // Внешние ключи и связи
        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
       
    }
}
