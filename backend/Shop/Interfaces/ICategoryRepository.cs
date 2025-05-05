using Shop.Model;

namespace Shop.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> FindByCategoryTitleAsync(string title , CancellationToken cancellationToken);
    }
}
