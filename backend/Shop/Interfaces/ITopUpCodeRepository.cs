using Shop.Enum;
using Shop.Model;

namespace Shop.Interfaces
{
    public interface ITopUpCodeRepository
    {
        Task<TopUpCode?> GetValidCodeAsync(string code, CancellationToken cancellationToken);
        Task UseCodeAsync(TopUpCode topUpCode, int userId, CancellationToken cancellationToken);
        Task<TopUpCode> CreateNewCodeAsync(TopUpAmount amount, CancellationToken cancellationToken);
    }
}
