using Shop.Enum;

namespace Shop.Model
{
    public class TopUpCode
    {
        public int TopUpCodeId { get; set; }

        public string Code { get; set; } = string.Empty;

        public TopUpAmount Amount { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? UsedByUserId { get; set; }
        public User? UsedByUser { get; set; }
    }
}
