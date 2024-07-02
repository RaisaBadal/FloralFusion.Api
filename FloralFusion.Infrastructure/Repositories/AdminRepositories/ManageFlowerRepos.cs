using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.AdminRepositories
{
    public class ManageFlowerRepos(FloralFusionDb floralFusionDb)
        : AbstractRepos<Flower>(floralFusionDb), IManageFlowers
    {
        #region AddFlowerCategoryAsync
        public async Task<bool> AddFlowerCategoryAsync(string name)
        {
            try
            {
                var fl = await floralFusionDb.FlowerCategory.Where(i => i.CategoryName == name).SingleOrDefaultAsync();
                if (fl != null)
                {
                    return false;
                }

                floralFusionDb.FlowerCategory.Add(new FlowerCategory
                {
                    CategoryName = name
                });
                await floralFusionDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                await Console.Out.WriteLineAsync(ex.StackTrace);
                return false;
            }
        }
        #endregion

        #region AddFlowerOccasionAsync
        public async Task<bool> AddFlowerOccasionAsync(string name)
        {
            try
            {
                var fl = await floralFusionDb.FlowerOccasion.Where(i => i.OccasionType == name).SingleOrDefaultAsync();
                if (fl != null)
                {
                    return false;
                }

                floralFusionDb.FlowerOccasion.Add(new FlowerOccasion
                {
                    OccasionType = name
                });
                await floralFusionDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                await Console.Out.WriteLineAsync(ex.StackTrace);
                return false;
            }
        }
        #endregion
    }
}
