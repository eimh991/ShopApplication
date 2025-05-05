using Shop.DTO;
using Shop.Model;

namespace Shop.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryDTO entity , CancellationToken cancellationToken);
        Task UpdateAsync(CategoryDTO entity, CancellationToken cancellationToken);
        Task<Category> GetByIdAsync(int id , CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAllAsync(string search, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
