namespace Shop.Interfaces
{
    public interface IRepositoryWithUser<T> where T : class
    {
        Task<T> GetByIdAsync(int userId, int entityId, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync( int userId, CancellationToken cancellationToken);
        Task AddAsync(int userId, T entity , CancellationToken cancellationToken);
        Task AddRangeAsync(int userId, List<T> entitys, CancellationToken cancellationToken);
        Task UpdateAsync(int userId, T entity , CancellationToken cancellationToken);
        Task DeleteAsync(int entityId, CancellationToken cancellationToken);


    }
}
