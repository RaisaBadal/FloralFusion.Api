using FloralFusion.Application.Models;

namespace FloralFusion.Application.Interfaces.Flowers
{
    public interface IFlowerService
    {
        Task<IEnumerable<FlowerModel>> GetFlowersByName(string name);

        Task<IEnumerable<FlowerModel>> GetFlowersByColor(string color);

        Task<IEnumerable<FlowerModel>> GetBetweenPrice(decimal price1, decimal price2);

        Task<IEnumerable<FlowerModel>> GetCategoryBetweenPrice(string category, decimal price1, decimal price2);

        Task<IEnumerable<FlowerModel>> GetByCategory(string category);

        Task<IEnumerable<FlowerModel>> GetByOccasion(string occasion);

        Task<IEnumerable<FlowerModel>> GetFeatured();

        Task<IEnumerable<FlowerModel>> GetAllAsync();

        Task<FlowerModel> GetByIdAsync(long id);
    }
}
