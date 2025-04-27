using Shop.Model;

namespace Shop.Interfaces
{
    public interface IUserAdditionalRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetUserWithCartsItemAsync(int userId);

        Task ChangeStatusAsync(int userId, string status);
    }
}
