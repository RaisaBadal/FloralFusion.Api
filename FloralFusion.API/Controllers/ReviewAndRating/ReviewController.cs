using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.ReviewAndRating;
using FloralFusion.Application.Models;
using FloralFusion.Application.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace FloralFusion.API.Controllers.ReviewAndRating
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {

        private readonly IReviewService reviewService;
        private readonly IMemoryCache memoryCache;

        public ReviewController(IReviewService reviewService, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.reviewService = reviewService;
        }

        [HttpPost]
        [Route(nameof(WriteReview))]
        public async Task<Response<long>> WriteReview(ReviewModel entity)
        {
            if(!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.NotFound);
            var res=await reviewService.Create(entity,userId);
            return res<0 ? Response<long>.Error(ErrorKeys.UnSuccessFullInsert)
                : Response<long>.Ok(res);
        }

        [HttpPatch]
        [Route("[action]/reviewId/{id}")]
        public async Task<Response<bool>> UpdateReview(long id, ReviewModel entity)
        {
            if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.NotFound);
            var res = await reviewService.Update(id, entity, userId);
            return res ? Response<bool>.Ok(res)
                : Response<bool>.Error(ErrorKeys.UnSuccessFullUpdate);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete]
        [Route("[action]/reviewId/{id}")]
        public async Task DeleteReviewById(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.NotFound);
            await reviewService.DeleteByIdAsync(id, userId);
        }

        [HttpPatch]
        [Route("[action]/reviewId/{id}")]
        public async Task<Response<bool>> SoftDeleteReviewById(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.NotFound);

            var res=await reviewService.SoftDeleteByIdAsync(id,userId);

            return res ? Response<bool>.Ok(res)
              : Response<bool>.Error(ErrorKeys.UnSuccessFullUpdate);
        }

        //shesuli momxmareblis reviewebis istorias wamoighebs 
        [HttpGet]
        [Route(nameof(GetAllReviewUsers))]
        public async Task<Response<IEnumerable<ReviewModel>>> GetAllReviewUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException(ErrorKeys.NotFound);

            var cacheKey = $"Users: {userId} review";
            if(memoryCache.TryGetValue(cacheKey, out IEnumerable<ReviewModel>? res)&& res.Any())
                return Response<IEnumerable<ReviewModel>>.Ok(res);  

            var result=await reviewService.GetAllReviewUsersAsync(userId);
            if(!result.Any()) return Response<IEnumerable<ReviewModel>>.Error(ErrorKeys.NotFound);

            memoryCache.Set(cacheKey, result,TimeSpan.FromMinutes(15));
            return Response<IEnumerable<ReviewModel>>.Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetAllReview))]
        public async Task<Response<IEnumerable<ReviewModel>>> GetAllReview()
        {
            const string cacheKey = $"all review";
            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<ReviewModel>? res) && res.Any())
                return Response<IEnumerable<ReviewModel>>.Ok(res);

            var result = await reviewService.GetAllAsync();
            if (!result.Any()) return Response<IEnumerable<ReviewModel>>.Error(ErrorKeys.NotFound);

            memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(15));
            return Response<IEnumerable<ReviewModel>>.Ok(result);
        }

        //admins ekutvis mocemuli service
        [Authorize(Roles ="ADMIN")]
        [HttpPost]
        [Route(nameof(GetReviewById))]
        public async Task<Response<ReviewModel>> GetReviewById(long id)
        {
            var res = await reviewService.GetByIdAsync(id);
            if(res is null) return  Response<ReviewModel>.Error(ErrorKeys.NotFound);

            return Response<ReviewModel>.Ok(res);

        }
    }
}
