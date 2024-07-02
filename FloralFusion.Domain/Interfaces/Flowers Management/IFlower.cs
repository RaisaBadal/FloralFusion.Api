using FloralFusion.Domain.Entities;

namespace FloralFusion.Domain.Interfaces.Flowers_Management
{
    public interface IFlower
    {
        Task<IEnumerable<Flower>> GetFlowersByName(string name);

        Task<IEnumerable<Flower>> GetFlowersByColor(string color);

        Task<IEnumerable<Flower>>GetBetweenPrice(decimal price1,decimal price2);

        Task<IEnumerable<Flower>> GetCategoryBetweenPrice(string category, decimal price1, decimal price2);

        Task<IEnumerable<Flower>> GetByCategory(string category);

        Task<IEnumerable<Flower>>GetByOccasion(string occasion);

        Task<IEnumerable<Flower>> GetFeatured();

    }
}
