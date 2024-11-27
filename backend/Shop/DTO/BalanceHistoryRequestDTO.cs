namespace Shop.DTO
{
    public class BalanceHistoryRequestDTO
    {
        public int BalanceHistoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int UsersId {  get; set; }  
    }
}
