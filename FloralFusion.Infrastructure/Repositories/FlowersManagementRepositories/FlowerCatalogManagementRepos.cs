using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Flowers_Management;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.FlowersManagementRepositories
{
    public class FlowerCatalogManagementRepos(FloralFusionDb floralFusionDb) : AbstractRepos<Flower>(floralFusionDb), IFlowerCatalogManagement
    {

        #region Create
        public async Task<long> Create(Flower entity)
        {
            ArgumentNullException.ThrowIfNull(entity,nameof(entity));
            await dbset.AddAsync(entity);
            await floralFusionDb.SaveChangesAsync();
            var res = await dbset.MaxAsync(i => i.Id);
            return res;
        }

        #endregion

        //admin
        #region DeleteByIdAsync

        public async Task DeleteByIdAsync(long id)
        {
            if(id<0) throw new ArgumentException("Invalid id");
            var res = await dbset.FirstOrDefaultAsync(i => i.Id == id)
                      ?? throw new InvalidOperationException($"No flower found by id: {id}");
            dbset.Remove(res);
            await floralFusionDb.SaveChangesAsync();

        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Flower>> GetAllAsync()
        {
            return await dbset.AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetByIdAsync

        public async Task<Flower> GetByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException("Invalid id");
            var res = await dbset.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id)
                      ?? throw new ArgumentException($"No flower found by id: {id}");
            return res;
        }
        #endregion

        #region SoftDeleteByIdAsync
        public async Task<bool> SoftDeleteByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException("Invalid id");
            var res = await dbset.FirstOrDefaultAsync(i => i.Id == id)
                      ?? throw new ArgumentException($"No flower found by id: {id}");
            res.IsActive = false;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Update
        public async Task<bool> Update(long id, Flower entity)
        {
            if (id < 0) throw new ArgumentException("Invalid id");
            var res = await dbset.FirstOrDefaultAsync(i => i.Id == id)
                      ?? throw new ArgumentException($"No flower found by id: {id}");
            res.Name=entity.Name;
            res.Color=entity.Color;
            res.Price=entity.Price;
            res.Description=entity.Description;
            res.Availability=entity.Availability;
            res.Featured=entity.Featured;
            res.Quantity = entity.Quantity;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
