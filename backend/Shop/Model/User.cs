using Shop.Enum;
using System.Data;

namespace Shop.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public decimal Balance { get; set; } // Текущий баланс пользователя

        public UserRole UserRole { get; set; }

        // Навигационные свойства
        public List<Order> Orders { get; set; } = new List<Order>();
        public Cart Cart { get; set; }
        public List<BalanceHistory> BalanceHistories { get; set; } = new List<BalanceHistory>();
    }
}
