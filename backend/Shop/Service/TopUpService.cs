using Microsoft.EntityFrameworkCore;
using Shop.Enum;
using Shop.Interfaces;
using Shop.Model;

namespace Shop.Service
{
    public class TopUpService : ITopUpService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepositoryWithUser<BalanceHistory> _balanceHistoryRepository;
        private readonly ITopUpCodeRepository _codeRepo;
        private readonly IUserBalanceUpdater _userBalanceUpdater;

        public TopUpService(
        ITopUpCodeRepository codeRepo,
        IRepository<User> userRepository,
        IRepositoryWithUser<BalanceHistory> balanceHistoryRepository,
        IUserBalanceUpdater userBalanceUpdater)
        {
            _codeRepo = codeRepo;
            _userRepository = userRepository;
            _balanceHistoryRepository = balanceHistoryRepository;
            _userBalanceUpdater = userBalanceUpdater;
        }

        public async Task<bool> ApplyTopUpCodeAsync(string code, int userId, CancellationToken cancellationToken)
        {
            var topUpCode = await _codeRepo.GetValidCodeAsync(code, cancellationToken );
            if (topUpCode == null)
                return false;

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                return false;

            user.Balance += (decimal)topUpCode.Amount;
            await _codeRepo.UseCodeAsync(topUpCode, userId, cancellationToken);

            var history = new BalanceHistory
            {
                Amount = (decimal)topUpCode.Amount,
                Description = $"Пополнение через код {topUpCode.Code}",
                TransactionDate = DateTime.UtcNow,
                UserId = userId
            };
            await _balanceHistoryRepository.AddAsync(userId, history, cancellationToken);

            await _userBalanceUpdater.UpdateBalanceAsync(user, cancellationToken);

            return true;
        }

        public async Task<string> CreateTopUpCodeAsync(decimal amountValue, CancellationToken cancellationToken)
        {
            if (amountValue % 1 != 0)
                throw new ArgumentException("Сумма должна быть целым числом");

            int intAmount = (int)amountValue;

            if (!System.Enum.IsDefined(typeof(TopUpAmount), intAmount))
                throw new ArgumentException($"Недопустимая сумма пополнения: {amountValue}");

            var amount = (TopUpAmount)amountValue;

            var newCode = await _codeRepo.CreateNewCodeAsync(amount, cancellationToken);

            return newCode.Code;
        }
    }
}
