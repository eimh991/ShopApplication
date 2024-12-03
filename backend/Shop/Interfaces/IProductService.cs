using Shop.DTO;
using Shop.Model;


namespace Shop.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductDTO entity);
        Task UpdateAsync(ProductDTO entity);
        Task<ProductResponceDTO> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponceDTO>> GetAllAsync(string search, int paginateSize, int page, string sortOrder);
        Task DeleteAsync(int id);
        
    }
}
