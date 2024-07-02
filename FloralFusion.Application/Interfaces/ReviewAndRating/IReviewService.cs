using FloralFusion.Application.Models;

namespace FloralFusion.Application.Interfaces.ReviewAndRating
{
    public interface IReviewService
    {
        Task<long> Create(ReviewModel entity, string userId);
        Task<bool> Update(long id, ReviewModel entity, string userId);
        Task DeleteByIdAsync(long id, string userId);
        Task<bool> SoftDeleteByIdAsync(long id, string userId);
        Task<IEnumerable<ReviewModel>> GetAllReviewUsersAsync(string userId);
        Task<IEnumerable<ReviewModel>> GetAllAsync();
        Task<ReviewModel> GetByIdAsync(long id);
    }
}
