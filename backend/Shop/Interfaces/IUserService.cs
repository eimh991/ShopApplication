using Shop.Model;
using Shop.UsersDTO;

namespace Shop.Interfaces
{
    public interface IUserService 
    {
        Task CreateAsync(UserDTO entity);
        Task UpdateAsync(UserDTO entity);
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync(string search);
        Task DeleteAsync(int id);
        Task<User> GetByEmaiAsync(string email);
        Task ChangeStatusAsync ( int userId , string status);

        Task<string> Login(string email, string password);
        Task<IEnumerable<CartItem>> GetUserCartItemsAsync(int userId);
    }
}
