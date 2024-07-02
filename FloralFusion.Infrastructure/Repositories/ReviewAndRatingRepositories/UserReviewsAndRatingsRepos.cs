using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Review_and_Rating_System;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.ReviewAndRatingRepositories
{
    public class UserReviewsAndRatingsRepos(FloralFusionDb floralFusionDb) :AbstractRepos<Reviews>(floralFusionDb),IUserReviewsAndRatings
    {
        #region GetByIdAsync

        public async Task<Reviews> GetByIdAsync(long id)
        {
            var res = await dbset.FindAsync(id)
                      ?? throw new ArgumentException($"No review found by id: {id}");
            return res;
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<Reviews>> GetAllAsync()
        {
            return await dbset.ToListAsync();
        }
        #endregion

        #region Create

        public async Task<long> Create(Reviews entity)
        {
            ArgumentNullException.ThrowIfNull(entity,nameof(entity));
            await dbset.AddAsync(entity);
            await floralFusionDb.SaveChangesAsync();
            var res = await dbset.MaxAsync(i => i.Id);
            return res;
        }
        #endregion

        #region Update

        public async Task<bool> Update(long id, Reviews entity)
        {
            var res = await dbset.FindAsync(id)
                      ?? throw new InvalidOperationException($"No review found by id: {id}");
            res.Text=entity.Text;
            res.UpdateAt=DateTime.Now;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }

        #endregion

        #region DeleteByIdAsync

        public async Task DeleteByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException("Invalid id");
            var res = await dbset.FindAsync(id)
                      ?? throw new ArgumentException($"No review found by id: {id}");
            dbset.Remove(res);
            await floralFusionDb.SaveChangesAsync();
        }
        #endregion

        #region SoftDeleteByIdAsync
        public async Task<bool> SoftDeleteByIdAsync(long id)
        {
            var res = await dbset.FindAsync(id)
                      ?? throw new ArgumentException($"No review found by id: {id}");
            res.IsActive = false;
            await floralFusionDb.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
