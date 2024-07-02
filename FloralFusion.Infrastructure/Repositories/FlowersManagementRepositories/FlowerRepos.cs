using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Flowers_Management;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.FlowersManagementRepositories
{
    public class FlowerRepos(FloralFusionDb floralFusionDb)
        : AbstractRepos<Flower>(floralFusionDb), IFlower
    {
        #region GetFlowersByName
        public async Task<IEnumerable<Flower>> GetFlowersByName(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            var flower = await dbset.Where(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
            if (flower.Count == 0) throw new ArgumentException($"No flower found by given name: {name}");
            return flower;
        }
        #endregion

        #region GetFlowersByColor

        public async Task<IEnumerable<Flower>> GetFlowersByColor(string color)
        {
            ArgumentNullException.ThrowIfNull(color);
            var flower = await dbset.Where(i => i.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
            if (flower.Count == 0) throw new ArgumentException($"No flower found with color: {color}");
            return flower;
        }
        #endregion

        #region GetBetweenPrice
        public async Task<IEnumerable<Flower>> GetBetweenPrice(decimal price1, decimal price2)
        {
            if (price1 < 0 || price2 < 0) throw new ArgumentException("Invalid price");
            var flower = await dbset.Where(i => i.Price >= price1 && i.Price <= price2).ToListAsync();
            if (!flower.Any())
                throw new InvalidOperationException($"No flower found between given price: {price1} and {price2}");
            return flower;
        }
        #endregion

        #region GetCategoryBetweenPrice
        public async Task<IEnumerable<Flower>> GetCategoryBetweenPrice(string category, decimal price1, decimal price2)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));
            if (price1 < 0 || price2 < 0) throw new ArgumentException("Invalid price");
            var flowerCategory = await floralFusionDb.FlowerCategory.Where(i => i.CategoryName
                                         .Equals(category, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync()
                                 ?? throw new ArgumentException($"No flower found by category: {category}");
            var flower = await dbset.Where(i => i.CategoryId == flowerCategory.Id && i.Price >= price1 && i.Price <= price2)
                .ToListAsync();
            if (flower.Any()) return flower;
            throw new InvalidOperationException("No flower found by this filters");
        }

        #endregion

        #region GetByCategory

        public async Task<IEnumerable<Flower>> GetByCategory(string category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));
            var flowerCategory = await floralFusionDb.FlowerCategory.Where(i => i.CategoryName
                                     .Equals(category, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync()
                                 ?? throw new ArgumentException($"No category found: {category}");
            var flowers = await dbset.Where(i => i.CategoryId == flowerCategory.Id).ToListAsync();
            if (flowers.Count == 0) throw new ArgumentException("No flowers found");
            return flowers;
        }
        #endregion

        #region GetByOccasion
        public async Task<IEnumerable<Flower>> GetByOccasion(string occasion)
        {
            ArgumentNullException.ThrowIfNull(occasion, nameof(occasion));
            var flowerOccasion = await floralFusionDb.FlowerOccasion.Where(i => i.OccasionType
                                     .Equals(occasion, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync()
                                 ?? throw new ArgumentException($"No occasion found: {occasion}");
            var flowers = await dbset.Where(i => i.OccasionId == flowerOccasion.Id).ToListAsync();
            if (flowers.Count == 0) throw new ArgumentException("No flowers found");
            return flowers;
        }
        #endregion

        #region GetFeatured
        public async Task<IEnumerable<Flower>> GetFeatured()
        {
            return await dbset.Where(i => i.Featured).ToListAsync();
        }
        #endregion

      
    }
}
