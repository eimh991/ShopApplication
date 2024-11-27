using System.ComponentModel.DataAnnotations;

namespace Shop.DTO
{
    public record LoginUserDTO (
        [Required]string Email,
        [Required]string Password);
    
}
