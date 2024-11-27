namespace Shop.Interfaces
{
    public interface IRepositoryWithUser<T> where T : class
    {
        Task<T> GetByIdAsync(int userId, int entityId);
        Task<IEnumerable<T>> GetAllAsync( int userId);
        Task AddAsync(int userId, T entity);
        Task AddRangeAsync(int userId, List<T> entitys);
        Task UpdateAsync(int userId, T entity);
        Task DeleteAsync(int entityId);


    }
}
