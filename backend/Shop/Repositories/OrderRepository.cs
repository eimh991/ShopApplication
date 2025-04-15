using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Interfaces;
using Shop.Model;


namespace Shop.Repositories
{
    public class OrderRepository : IRepositoryWithUser<Order>
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(int userId, Order entity)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                user.Orders.Add(entity);
                await _context.SaveChangesAsync();
            }

        }

        public Task AddRangeAsync(int userId, List<Order> entities)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int entityId)
        {
            await _context.Orders
                .Where(o=>o.OrderId == entityId)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();
        }

        public  async Task<IEnumerable<Order>> GetAllAsync(int userId)
        {
                return await _context.Orders
                    .AsNoTracking()
                    .Include(o=>o.OrderItems)
                        .ThenInclude(oi=>oi.Product)
                    .Where(o=>o.UserId == userId)
                    .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int userId, int entityId)
        {
            return await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.OrderItems)
                    .Where(o => o.UserId == userId)
                    .FirstOrDefaultAsync(o => o.OrderId == entityId) 
                     ?? throw new Exception(message: "Заказ который вы ищите не существует");

            /*
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                return user.Orders
                    .FirstOrDefault(o=>o.OrderId == entityId)
                    ?? throw new Exception(message: "Заказ который вы ищите не существует");
            }
            return null;
            */
        }

        public Task UpdateAsync(int userId, Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
