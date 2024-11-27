namespace Shop.Model
{
    public class Cart
    {
        public int CartId { get; set; }

        // Внешний ключ и связь
        public int UserId { get; set; }
        public User User { get; set; } // Связь один к одному с User

        // Навигационные свойства
        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Связь один ко многим с CartItem
    }
}
