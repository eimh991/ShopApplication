using System.ComponentModel.DataAnnotations;

namespace Shop.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        [Required]
        public string CategoryTitle { get; set; } = string.Empty;
    }
}
