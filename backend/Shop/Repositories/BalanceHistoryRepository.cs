using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Interfaces;
using Shop.Model;

namespace Shop.Repositories
{
    public class BalanceHistoryRepository : IRepositoryWithUser<BalanceHistory>
    {
        private readonly AppDbContext _context;
        public BalanceHistoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(int userId, BalanceHistory entity, CancellationToken cancellationToken)
        {
            var user =  await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                user.BalanceHistories.Add(entity);
                user.Balance += entity.Amount;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddRangeAsync(int userId, List<BalanceHistory> entitys, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                for (int i = 0; i < entitys.Count; i++)
                {
                    user.BalanceHistories.Add(entitys[i]);
                    user.Balance += entitys[i].Amount;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(int entityId, CancellationToken cancellationToken)
        {
            await _context.BalanceHistorys
                .Where(b=>b.BalanceHistoryId == entityId)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<BalanceHistory>> GetAllAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if(user != null)
            {
                return user.BalanceHistories;
            }
            return Enumerable.Empty<BalanceHistory>();
        }

        public async Task<BalanceHistory> GetByIdAsync(int userId, int entityId, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                return user.BalanceHistories.
                    FirstOrDefault(b => b.BalanceHistoryId == entityId) 
                    ?? throw new Exception(message : "У данного пользователя нету такой транзакции");
            }
            return null;
        }

        public Task UpdateAsync(int userId, BalanceHistory entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
