using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Payment_Processing;
using Microsoft.EntityFrameworkCore;
using ArgumentException = System.ArgumentException;

namespace FloralFusion.Infrastructure.Repositories.PaymentMethodRepositories
{
    public class PaymentRepos(FloralFusionDb floralFusionDb) : AbstractRepos<PaymentMethod>(floralFusionDb),IPaymentMethods
    {
        #region GetByIdAsync

        public async Task<PaymentMethod> GetByIdAsync(long id)
        {
            var res = await dbset.FindAsync(id)
                      ?? throw new InvalidOperationException($"No payment method found by id: {id}");
            return res;
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
        {
            return await dbset.AsNoTracking().ToListAsync();
        }
        #endregion

        #region Create

        public async Task<long> Create(PaymentMethod entity)
        {
            if (entity.MethodName == null) throw new ArgumentException("method name is empty");
            await dbset.AddAsync(entity);
            await floralFusionDb.SaveChangesAsync();
            var res = await dbset.MaxAsync(i => i.Id);
            return res;
        }
        #endregion

        #region Update

        public async Task<bool> Update(long id, PaymentMethod entity)
        { 
            var payment = await dbset.FindAsync(id)
                          ?? throw new ArgumentException($"No method found by id: {id}");
            payment.MethodName=entity.MethodName;
            payment.Description=entity.Description;
            payment.FeePercentage=entity.FeePercentage;
            payment.MaxAmount = entity.MaxAmount;
            payment.MinAmount = entity.MinAmount;
            payment.UpdatedAt=DateTime.Now;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion

        #region DeleteByIdAsync
        public async Task DeleteByIdAsync(long id)
        {
            var res = await dbset.FindAsync(id)
                      ?? throw new ArgumentException($"No payment method found by id: {id}");
            dbset.Remove(res);
        }
        #endregion

        #region SoftDeleteByIdAsync

        public async Task<bool> SoftDeleteByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var res = await dbset.FindAsync(id)
                      ?? throw new ArgumentException($"No payment method found by id: {id}");
            res.IsActive = false;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
