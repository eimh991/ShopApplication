namespace Shop.Model
{
    public class BalanceHistory
    {
        public int BalanceHistoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }

        // Внешний ключ и связь
        public int UserId { get; set; }
        public User? User { get; set; } // Связь многие к одному с User
    }
}
