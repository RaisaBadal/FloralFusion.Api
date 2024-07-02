using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.DeliveryManagement;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.DeliveryManagementRepositories
{
    public class DeliveryOptionsRepositories(FloralFusionDb floralFusionDb)
        : AbstractRepos<DeliveryOption>(floralFusionDb), IDeliveryOptions
    {
        #region Create
        public async Task<long> Create(DeliveryOption entity)
        {
            var delivery = await dbset.AnyAsync(i => i.OptionName == entity.OptionName);
            if (delivery) throw new InvalidOperationException("A similar delivery option is already exist");
            
            await dbset.AddAsync(entity);
            await floralFusionDb.SaveChangesAsync();
            var res = await dbset.MaxAsync(i => i.Id);
            return res;

        }
        #endregion

        #region DeleteByIdAsync

        public async Task DeleteByIdAsync(long id)
        {
           if(id < 0) throw new ArgumentException($"Invalid id: {id}");
           var option = await dbset.FirstOrDefaultAsync(i => i.Id == id) 
                        ?? throw new InvalidOperationException($"No option found by id: {id}"); 
           dbset.Remove(option);
           await floralFusionDb.SaveChangesAsync();
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<DeliveryOption>> GetAllAsync()
        {
            return await dbset.ToListAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<DeliveryOption> GetByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException($"Invalid id for delivery option: {id}");
            var option = await dbset.FirstOrDefaultAsync(i => i.Id == id)
                         ?? throw new ArgumentException($"No delivery option found by id {id}");
            return option;
        }
        #endregion

        #region SoftDeleteByIdAsync

        public async Task<bool> SoftDeleteByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var option = await dbset.FirstOrDefaultAsync(i => i.Id == id)
                         ?? throw new InvalidOperationException($"No option found by id: {id}");
            option.IsActive=false;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Update

        public async Task<bool> Update(long id, DeliveryOption entity)
        {
            ArgumentNullException.ThrowIfNull(entity,nameof(entity));
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var option = await dbset.FirstOrDefaultAsync(i => i.Id == id)
                         ?? throw new ArgumentException($"No delivery option find by id: {id}");
            option.OptionName=entity.OptionName;
            option.Description = entity.Description;
            option.Price=entity.Price;
            option.MinDeliveryTime=entity.MinDeliveryTime;
            option.MaxDeliveryTime=entity.MaxDeliveryTime;
            option.AdditionalInformation=entity.AdditionalInformation;
            option.UpdatedAt=DateTime.Now;
            await floralFusionDb.SaveChangesAsync();
            return true;

        }
        #endregion
    }
}
