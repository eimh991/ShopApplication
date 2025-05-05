using Shop.DTO;
using Shop.Model;


namespace Shop.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductRequestDTO entity , CancellationToken cancellationToken);
        Task UpdateAsync(ProductRequestDTO entity, CancellationToken cancellationToken);
        Task<ProductResponceDTO> GetByIdAsync(int id , CancellationToken cancellationToken);
        Task<IEnumerable<ProductResponceDTO>> GetAllAsync(string search, int paginateSize, 
                            int page, string sortOrder, string categoryName , CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        
    }
}
