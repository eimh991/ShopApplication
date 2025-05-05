namespace Shop.Interfaces
{
    public interface ICartItemCleaner
    {
        Task DeleteAllCartItemsAsync(int userId, CancellationToken cancellationToken);
    }
}
