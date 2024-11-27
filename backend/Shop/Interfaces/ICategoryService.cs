using Shop.DTO;
using Shop.Model;

namespace Shop.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryDTO entity);
        Task UpdateAsync(CategoryDTO entity);
        Task<Category> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync(string search);
        Task DeleteAsync(int id);
    }
}
