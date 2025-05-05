namespace Shop.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity , CancellationToken cancellationToken);
        Task<T> GetByIdAsync(int id , CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(string search , CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
 
    }
}
