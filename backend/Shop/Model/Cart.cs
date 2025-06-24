namespace Shop.Model
{
    public class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } 

        // Навигационные свойства
        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Связь один ко многим с CartItem
    }
}
