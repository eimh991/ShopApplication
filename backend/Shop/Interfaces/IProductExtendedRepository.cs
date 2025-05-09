using Shop.Model;

namespace Shop.Interfaces
{
    public interface IProductExtendedRepository
    {
        Task<IEnumerable<Product>> GetAllPaginateAsync(string search, int paginateSize, int page, string sortOrder, int categoryId, CancellationToken cancellationToken);
        Task ChangePriceAsync(Product product, CancellationToken cancellationToken);
        Task ChangeQuantityProductAsync(Product product, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetLastProductsAsync(CancellationToken cancellationToken);
        Task ChangeImagePathProductAsync(Product product, CancellationToken cancellationToken);
    }
}
