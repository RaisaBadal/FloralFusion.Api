using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.ReviewAndRating;
using FloralFusion.Application.Models;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;

namespace FloralFusion.Application.Services.ReviewAndRatingServices
{
    public class ReviewService : AbstractClass, IReviewService
    {
        public ReviewService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService) : base(uniteOfWork, mapper, smtpService)
        {
        }

        #region Create
        public async Task<long> Create(ReviewModel entity, string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var mapped = mapper.Map<Reviews>(entity)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                mapped.UserId = userId;
                var res = await uniteOfWork.UserReviewsAndRatings.Create(mapped);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

        #region DeleteByIdAsync
        public async Task DeleteByIdAsync(long id, string userId)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var res = await uniteOfWork.UserReviewsAndRatings.GetByIdAsync(id);
                if (res.UserId != userId) throw new GeneralException(ErrorKeys.BadRequest);
                await uniteOfWork.UserReviewsAndRatings.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

        #region GetAllReviewUsersAsync

        public async Task<IEnumerable<ReviewModel>> GetAllReviewUsersAsync(string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var res = await uniteOfWork.UserReviewsAndRatings.GetAllAsync();
                var userAllReviews = res.Where(i => i.UserId == userId).ToList();
                if (!userAllReviews.Any()) throw new GeneralException(ErrorKeys.NotFound);
                var mapped = mapper.Map<IEnumerable<ReviewModel>>(userAllReviews)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                return mapped;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<ReviewModel>> GetAllAsync()
        {
            try
            {
                var res = await uniteOfWork.UserReviewsAndRatings.GetAllAsync()
                    ?? throw new InvalidOperationException(ErrorKeys.NotFound);
                var mapped=mapper.Map<IEnumerable<ReviewModel>>(res)
                     ?? throw new InvalidOperationException(ErrorKeys.Mapped);
                return mapped;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        //admin
        #region GetByIdAsync
        public async Task<ReviewModel> GetByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException("Invalid id");
                var res = await uniteOfWork.UserReviewsAndRatings.GetByIdAsync(id)
                    ?? throw new InvalidOperationException(ErrorKeys.NotFound);
                var mapped=mapper.Map<ReviewModel>(res)
                       ?? throw new InvalidOperationException(ErrorKeys.Mapped);
                return mapped;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region SoftDeleteByIdAsync
        public async Task<bool> SoftDeleteByIdAsync(long id, string userId)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var fl = await uniteOfWork.UserReviewsAndRatings.GetByIdAsync(id);
                if (fl.UserId != userId) throw new GeneralException(ErrorKeys.BadRequest);
                var res = await uniteOfWork.UserReviewsAndRatings.SoftDeleteByIdAsync(id);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

        #region Update
        public async Task<bool> Update(long id, ReviewModel entity, string userId)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var mapped = mapper.Map<Reviews>(entity)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                mapped.UserId = userId;
                var res = await uniteOfWork.UserReviewsAndRatings.Update(id, mapped);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion
    }
}
