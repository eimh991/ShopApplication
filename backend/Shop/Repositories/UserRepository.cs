using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shop.Data;
using Shop.Enum;
using Shop.Interfaces;
using Shop.Model;

namespace Shop.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public  async Task CreateAsync(User entity)
        {
            entity.Cart = new Cart();

            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            
            if (user != null)
            {
                _context.Users.Remove(user);

                await _context.SaveChangesAsync();
            }
            //как альтернатива
            //await _context.Users.Where(u=>u.UserId == id).ExecuteDeleteAsync();

        }

        public async Task<IEnumerable<User>> GetAllAsync(string search)
        {
            var users = await _context.Users
                .AsNoTracking()
                .Include(u=>u.Orders)
                .Where(u=>!string.IsNullOrWhiteSpace(search) ||
                u.UserName.ToLower().Contains(search.ToLower())).ToListAsync();

            if (users != null)
            {
                return users;
            }
            return new List<User>() ;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u=>u.Orders)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task UpdateAsync(User entity)
        {
            await _context.Users
                .Where(u => u.UserId == entity.UserId)
                .ExecuteUpdateAsync(s =>s
                    .SetProperty(u => u.UserName, entity.UserName)
                    .SetProperty(u => u.Email, entity.Email)
                    .SetProperty(u => u.PasswordHash, entity.PasswordHash)
                );
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task ChangeStatusAsync(int userId,string status)
        {
            var userRole = UserRole.User;
            if(UserRole.Admin.ToString().ToLower() == status.ToLower())
            {
                userRole = UserRole.Admin;
            }
            else if(UserRole.Manager.ToString().ToLower() == status.ToLower())
            {
                userRole = UserRole.Manager;
            }

            await _context.Users
                .Where(u => u.UserId == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.UserRole, userRole)
                    );
        }

        public async Task<User> GetUserWithCartAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<User> GetUserWithCartsItemAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.Cart.CartItems)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}
