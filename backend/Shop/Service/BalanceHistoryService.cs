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
        public async Task CreateBalanceHistoryAsync(BalanceHistoryRequestDTO entity)
        {
            var user = await _userRepository.GetByIdAsync(entity.UsersId);
            if (user != null) {
                var balance = new BalanceHistory
                {
                    BalanceHistoryId = entity.BalanceHistoryId,
                    Amount = entity.Amount,
                    Description = entity.Description,
                    TransactionDate = DateTime.UtcNow,
                    User = user
                };
                await _balanceRepository.AddAsync(user.UserId, balance);
            }
            throw new Exception(message: "Не корректные данные");

        }

        public async Task DeleteBalsnceHistoryAsync(int balanceHistoryId)
        {
            await _balanceRepository.DeleteAsync(balanceHistoryId);
        }

        public async Task<IEnumerable<BalanceHistory>> GetAllHistoryAsync(int userId)
        {
            return await _balanceRepository.GetAllAsync(userId);
        }

        public async Task<BalanceHistory> GetBalanceHistoryByIdAsync(int userId,int balanceHistoryId)
        {
           return await _balanceRepository.GetByIdAsync(userId, balanceHistoryId);
        }

        public Task UpdateBalanceHistoryAsync(BalanceHistoryRequestDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
