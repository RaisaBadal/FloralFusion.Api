using FloralFusion.Application.Models;

namespace FloralFusion.Application.Interfaces.Flowers
{
    public interface IFlowerCatalogManagementService
    {
        Task<long> Create(FlowerModel entity, string userId);
        Task<bool> Update(long id, FlowerModel entity, string userId);
        Task DeleteByIdAsync(long id, string userId);
        Task<bool> SoftDeleteByIdAsync(long id, string userId);
        Task<IEnumerable<FlowerModel>> GetAllAsync(string userId);
        Task<FlowerModel> GetByIdAsync(long id,string userId);
    }
}
