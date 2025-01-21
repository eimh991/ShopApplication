using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Interfaces;
using Shop.Model;

namespace Shop.Repositories
{
    public class CartItemRepository : IRepositoryWithUser<CartItem>
    {
        private readonly AppDbContext _context;
        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(int userId, CartItem entity)
        {
            var user = await _context.Users
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user != null)
            {
                user.Cart.CartItems.Add(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new EntryPointNotFoundException();
            }
        }

        public async Task AddRangeAsync(int userId, List<CartItem> entitys)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user != null)
            {
                user.Cart.CartItems.AddRange(entitys);
                await _context.SaveChangesAsync();
            }
            throw new EntryPointNotFoundException();
        }

        public async Task DeleteAsync(int entityId)
        {
            await _context.CartItems
                .Where(ci=>ci.CartItemId == entityId)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync(int userId)
        {
            var user =  await _context.Users
                .AsNoTracking()
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(u => u.UserId == userId);
           
            if(user != null)
            return user.Cart.CartItems;

            return Enumerable.Empty<CartItem>();
        }

        public  async Task<CartItem> GetByIdAsync(int userId,int entityId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                return user.Cart.CartItems.FirstOrDefault(ci => ci.CartId == entityId) ?? throw new Exception(message: "У вас нету данного товара в корзине");
            }
            return null;

        }

        public async Task UpdateAsync(CartItem entity)
        {
            //var  user = await _context.Users
            //.AsNoTracking()
            //.FirstOrDefaultAsync(u => u.UserId == userId);

            //if (user != null)
            //{
            var cartItem = await _context.CartItems
                     .FirstOrDefaultAsync(c => c.CartItemId == entity.CartItemId);
                if (cartItem != null)
                {
                    cartItem.Quantity = entity.Quantity;
                    if (cartItem.Quantity == 0)
                    {
                        await DeleteAsync(cartItem.CartItemId);
                    }
                
                await _context.SaveChangesAsync();
            }
            //}
        }

        public async Task DeleteAllCartItemsAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                user.Cart.CartItems.Clear();
                await _context.SaveChangesAsync();
            }
        }

        public Task UpdateAsync(int userId, CartItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
