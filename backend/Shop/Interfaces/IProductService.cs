using Shop.DTO;
using Shop.Model;


namespace Shop.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductRequestDTO entity);
        Task UpdateAsync(ProductRequestDTO entity);
        Task<ProductResponceDTO> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponceDTO>> GetAllAsync(string search, int paginateSize, 
                            int page, string sortOrder, string categoryName);
        Task DeleteAsync(int id);
        
    }
}
