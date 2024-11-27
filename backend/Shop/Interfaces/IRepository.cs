namespace Shop.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(string search);
        Task DeleteAsync(int id);
 
    }
}
