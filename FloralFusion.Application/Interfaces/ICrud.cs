namespace FloralFusion.Application.Interfaces
{
    public interface ICrud<T,K> where T : class
    {
        Task<T> GetByIdAsync(K id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<K> CreateAsync(T entity);
        Task<bool> UpdateAsync(K id, T entity);
        Task DeleteByIdAsync(K id);
        Task<bool> SoftDeleteByIdAsync(K id);
    }
}
