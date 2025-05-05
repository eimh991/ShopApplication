using Shop.Model;

namespace Shop.Interfaces
{
    public interface IUserAdditionalRepository
    {
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User> GetUserWithCartsItemAsync(int userId, CancellationToken cancellationToken);

        Task ChangeStatusAsync(int userId, string status, CancellationToken cancellationToken);

        Task<User> GetUserWithCartAsync(int userId, CancellationToken cancellationToken);
    }
}
