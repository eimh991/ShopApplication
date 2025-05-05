using Shop.Model;

namespace Shop.Interfaces
{
    public interface IUserBalanceUpdater
    {
        Task UpdateBalanceAsync(User user, CancellationToken cancellationToken);
    }
}
