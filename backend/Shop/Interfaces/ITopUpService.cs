namespace Shop.Interfaces
{
    public interface ITopUpService
    {
        Task<bool> ApplyTopUpCodeAsync(string code, int userId, CancellationToken cancellationToken);

        Task<string> CreateTopUpCodeAsync(int amountValue, CancellationToken cancellationToken);
    }
}
