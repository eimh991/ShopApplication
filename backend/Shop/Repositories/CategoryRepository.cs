using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Interfaces;
using Shop.Model;
using System.Reflection.Metadata.Ecma335;

namespace Shop.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Category entity)
        {
            _context.Categories.Attach(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
             await _context.Categories
                .Where(c=>c.CategoryId == id)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string search)
        {
            var cat =  await _context.Categories
                .AsNoTracking()
                .Where(c=>!string.IsNullOrWhiteSpace(search) ||
                c.CategoryName.ToLower().Contains(search.ToLower()))
                .ToListAsync();
            return cat;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c=>c.CategoryId==id);
            return category ?? null;
        }

        public async Task UpdateAsync(Category entity)
        {
            await _context.Categories
                .Where(c => c.CategoryId == entity.CategoryId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(c=>c.CategoryName,entity.CategoryName)
                );
        }
        
        public async Task<Category> FindByCategoryTitlleAsync(string title)
        {
            return await _context.Categories
                     .FirstOrDefaultAsync(c => 
                     c.CategoryName.ToLower() == title.ToLower())
                     ?? null;
        }
    }
}
