using Shop.DTO;
using Shop.Model;

namespace Shop.Interfaces
{
    public interface IBalanceHistoryService
    {
        Task CreateBalanceHistoryAsync(BalanceHistoryRequestDTO entity, CancellationToken cancellationToken);
        Task UpdateBalanceHistoryAsync(BalanceHistoryRequestDTO entity, CancellationToken cancellationToken);
        Task<BalanceHistory> GetBalanceHistoryByIdAsync(int BalanceHistoryId, int userId, CancellationToken cancellationToken);
        Task<IEnumerable<BalanceHistory>> GetAllHistoryAsync(int userId, CancellationToken cancellationToken);
        Task DeleteBalsnceHistoryAsync(int BalanceHistoryId, CancellationToken cancellationToken);
    }
}
