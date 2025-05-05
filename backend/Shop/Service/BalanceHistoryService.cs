using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;

namespace Shop.Service
{
    public class BalanceHistoryService : IBalanceHistoryService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepositoryWithUser<BalanceHistory> _balanceRepository;
        public BalanceHistoryService(IRepository<User> userRepository, IRepositoryWithUser<BalanceHistory> balanceHistory)
        {
            _userRepository = userRepository;
            _balanceRepository = balanceHistory;
        }
        public async Task CreateBalanceHistoryAsync(BalanceHistoryRequestDTO entity, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(entity.UsersId, cancellationToken);
            if (user != null) {
                var balance = new BalanceHistory
                {
                    BalanceHistoryId = entity.BalanceHistoryId,
                    Amount = entity.Amount,
                    Description = entity.Description,
                    TransactionDate = DateTime.UtcNow,
                    User = user
                };
                await _balanceRepository.AddAsync(user.UserId, balance,cancellationToken);
            }
            throw new Exception(message: "Не корректные данные");

        }

        public async Task DeleteBalsnceHistoryAsync(int balanceHistoryId, CancellationToken cancellationToken)
        {
            await _balanceRepository.DeleteAsync(balanceHistoryId,cancellationToken);
        }

        public async Task<IEnumerable<BalanceHistory>> GetAllHistoryAsync(int userId, CancellationToken cancellationToken)
        {
            return await _balanceRepository.GetAllAsync(userId, cancellationToken);
        }

        public async Task<BalanceHistory> GetBalanceHistoryByIdAsync(int userId,int balanceHistoryId, CancellationToken cancellationToken)
        {
           return await _balanceRepository.GetByIdAsync(userId, balanceHistoryId, cancellationToken);
        }

        public Task UpdateBalanceHistoryAsync(BalanceHistoryRequestDTO entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
