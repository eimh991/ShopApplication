using Shop.Model;
using Shop.UsersDTO;

namespace Shop.Interfaces
{
    public interface IUserService 
    {
        Task CreateAsync(UserDTO entity, CancellationToken cancellationToken);
        Task UpdateAsync(UserDTO entity, CancellationToken cancellationToken);
        Task<UserDTO> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllAsync(string search , CancellationToken cancellationToken);
        Task DeleteAsync(int id , CancellationToken cancellationToken);
        Task<User> GetByEmaiAsync(string email, CancellationToken cancellationToken);
        Task ChangeStatusAsync ( int userId , string status, CancellationToken cancellationToken);

        Task<string> Login(string email, string password , CancellationToken cancellationToken);
        Task<IEnumerable<CartItem>> GetUserCartItemsAsync(int userId , CancellationToken cancellationToken);
    }
}
