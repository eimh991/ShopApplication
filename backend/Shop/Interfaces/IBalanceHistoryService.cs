using Shop.DTO;
using Shop.Model;

namespace Shop.Interfaces
{
    public interface IBalanceHistoryService
    {
        Task CreateBalanceHistoryAsync(BalanceHistoryRequestDTO entity);
        Task UpdateBalanceHistoryAsync(BalanceHistoryRequestDTO entity);
        Task<BalanceHistory> GetBalanceHistoryByIdAsync(int BalanceHistoryId, int userId);
        Task<IEnumerable<BalanceHistory>> GetAllHistoryAsync(int userId);
        Task DeleteBalsnceHistoryAsync(int BalanceHistoryId);
    }
}
