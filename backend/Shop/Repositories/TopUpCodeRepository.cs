using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Enum;
using Shop.Interfaces;
using Shop.Model;

namespace Shop.Repositories
{
    public class TopUpCodeRepository : ITopUpCodeRepository
    {
        private readonly AppDbContext _context;

        public TopUpCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TopUpCode?> GetValidCodeAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.TopUpCodes
                .FirstOrDefaultAsync(c => c.Code == code && !c.IsUsed);
        }

        public async Task UseCodeAsync(TopUpCode topUpCode, int userId, CancellationToken cancellationToken)
        {
            /*
            topUpCode.IsUsed = true;
            topUpCode.UsedByUserId = userId;
            topUpCode.CreatedAt = DateTime.UtcNow;
            _context.TopUpCodes.Update(topUpCode);
            */

            await _context.TopUpCodes
               .Where(t => t.TopUpCodeId== topUpCode.TopUpCodeId)
               .ExecuteUpdateAsync(s => s
                   .SetProperty(t => t.IsUsed, topUpCode.IsUsed)
                   .SetProperty(t => t.UsedByUserId, topUpCode.UsedByUserId)
                   .SetProperty(t => t.CreatedAt, topUpCode.CreatedAt)
               );
        }

        public async Task<TopUpCode> CreateNewCodeAsync(TopUpAmount amount, CancellationToken cancellationToken)
        {
            var code = GenerateUniqueCode(); 
            var newCode = new TopUpCode
            {
                Code = code,
                Amount = amount,
                IsUsed = false
            };
            await _context.TopUpCodes.AddAsync(newCode, cancellationToken);
            return newCode;
        }

        private string GenerateUniqueCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
        }
    }
}
