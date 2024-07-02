namespace FloralFusion.Domain.Interfaces
{
    public interface ICrud<T,K> where T: class
    {
        Task<T> GetByIdAsync (K id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<K> Create(T entity);
        Task<bool> Update(K id,T entity);
        Task DeleteByIdAsync (K id);
        Task<bool> SoftDeleteByIdAsync(K id);
    }
}
