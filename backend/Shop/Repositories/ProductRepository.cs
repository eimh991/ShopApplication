using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Interfaces;
using Shop.Model;

namespace Shop.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) {
            _context = context;
        }
        public async Task CreateAsync(Product entity)
        {
            await _context.Products.AddAsync(entity); //_context.Products.Attach(entity);
            await _context.SaveChangesAsync();  
        }

        public async Task DeleteAsync(int id)
        {   /*
            var product = await _context.Products.FirstOrDefaultAsync(p=>p.ProductId == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            */
            await _context.Products
               .Where(p => p.ProductId == id)
               .ExecuteDeleteAsync();
            
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string search)
        {
            return await _context.Products
                            .AsNoTracking()
                            .Where(p=> !string.IsNullOrWhiteSpace(search) ||
                            p.Name.ToLower().Contains(search.ToLower())).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product != null)
            {
                return product;
            }
            return null;


        }

        public  async Task UpdateAsync(Product entity)
        {
            await _context.Products
               .Where(p => p.ProductId == entity.ProductId)
               .ExecuteUpdateAsync(s => s
                   .SetProperty(p => p.Name, entity.Name)
                   .SetProperty(p => p.Description, entity.Description)
                   .SetProperty(p=>p.CategoryId, entity.CategoryId)
               );

        }

        public async Task ChangePriceAsync(Product entity)
        {
            await _context.Products
               .Where(p => p.ProductId == entity.ProductId)
               .ExecuteUpdateAsync(s => s
                   .SetProperty(p => p.Price, entity.Price)
               );
        }
        public async Task ChangeQuantityProductAsync(Product entity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == entity.ProductId);
            if (product != null)
            {
                product.Stock += entity.Stock;
                await _context.SaveChangesAsync();
            }
  
        }
        public async Task<IEnumerable<Product>> GetAllPaginateAsync(string search, int paginateSize, int page, string sortOrder)
        {
            var query = _context.Products
                            .AsNoTracking()
                            .Where(p => string.IsNullOrWhiteSpace(search) ||
                            p.Name.ToLower().Contains(search.ToLower()));
                            //.Skip(paginateSize * (page - 1))
                            //.Take(paginateSize);

            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                query = query.OrderBy(p => p.Price);
            }
            return await query
                .Skip(paginateSize * (page - 1))
                .Take(paginateSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLastProductsAsync()
        {
            var product =  await _context.Products
                            .AsNoTracking()
                            .OrderByDescending(p=>p.ProductId)
                            .Take(3)
                            .ToListAsync();

            return product;
        }
    }
}
