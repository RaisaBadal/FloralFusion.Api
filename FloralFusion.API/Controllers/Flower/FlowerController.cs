using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Interfaces.Flowers;
using FloralFusion.Application.Models;
using FloralFusion.Application.response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FloralFusion.API.Controllers.Flower
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowerController : ControllerBase
    {
        private readonly IFlowerService _flowerService;
        private readonly IMemoryCache _memoryCache;

        public FlowerController(IFlowerService _flowerService, IMemoryCache _memoryCache)
        {
            this._flowerService = _flowerService;
            this._memoryCache = _memoryCache;
        }

        [HttpPost]
        [Route("[action]/{name}")]
        public async Task<Response<IEnumerable<FlowerModel>>> GetByName([FromRoute]string name)
        {
            var cacheKey = $"Flowers by name: {name}";

            if(_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? memory)&& memory is not null) 
                return Response<IEnumerable<FlowerModel>>.Ok(memory);

            var res=await _flowerService.GetFlowersByName(name);
            if (!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res,TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{color}")]
        public async Task<Response<IEnumerable<FlowerModel>>> GetFlowersByColor([FromRoute]string color)
        {
            var cacheKey = $"Flowers by color: {color}";

            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? memory) && memory is not null)
                return Response<IEnumerable<FlowerModel>>.Ok(memory);

            var res = await _flowerService.GetFlowersByColor(color);
            if (!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{price1}/{price2}")]

        public async Task<Response<IEnumerable<FlowerModel>>> GetBetweenPrice([FromRoute]decimal price1, [FromRoute]decimal price2)
        {
            var cacheKey = $"Flowers between price: {price1} and {price2}";

            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? memory) && memory is not null)
                return Response<IEnumerable<FlowerModel>>.Ok(memory);

            var res = await _flowerService.GetBetweenPrice(price1,price2);
            if (!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);
        }

        [HttpPost]
        [Route("FlowerByCategory/{category}/betweenPrice/{price1}/{price2}")]
        public async Task<Response<IEnumerable<FlowerModel>>> GetCategoryBetweenPrice([FromRoute]string category, [FromRoute] decimal price1,[FromRoute] decimal price2)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentException(ErrorKeys.ArgumentNull);

            var cacheKey = $"Flowers by category: {category} between price: {price1} and {price2}";

            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? memory) && memory is not null)
                return Response<IEnumerable<FlowerModel>>.Ok(memory);

            var res = await _flowerService.GetCategoryBetweenPrice(category,price1, price2);
            if (!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{category}")]
        public async Task<Response<IEnumerable<FlowerModel>>> GetByCategory([FromRoute]string category)
        {
            var cacheKey = $"Flowers by category: {category}";

            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? memory) && memory is not null)
                return Response<IEnumerable<FlowerModel>>.Ok(memory);

            var res = await _flowerService.GetByCategory(category);
            if (!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{occasion}")]
        public async Task<Response<IEnumerable<FlowerModel>>> GetByOccasion(string occasion)
        {
            var cacheKey = $"Flowers by occasion: {occasion}";

            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? memory) && memory is not null)
                return Response<IEnumerable<FlowerModel>>.Ok(memory);

            var res = await _flowerService.GetByOccasion(occasion);
            if (!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<FlowerModel>>> FeaturedFlowers()
        {
            const string cacheKey = "FeaturedFlowers";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? memory) && memory.Any())
                return Response<IEnumerable<FlowerModel>>.Ok(memory);

            var res = await _flowerService.GetFeatured();
            if(!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey,res, TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<FlowerModel>>> GetAll()
        {
            const string cacheKey = "AllFlower";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<FlowerModel>? memory) && memory.Any())
                return Response<IEnumerable<FlowerModel>>.Ok(memory);

            var res = await _flowerService.GetAllAsync();
            if (!res.Any()) return Response<IEnumerable<FlowerModel>>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<IEnumerable<FlowerModel>>.Ok(res);
        }

        [HttpPost]
        [Route("[action]/{occasion}")]
        public async Task<Response<FlowerModel>> GetByIdAsync(long id)
        {
            var cacheKey = $"Flowers by id: {id}";

            if (_memoryCache.TryGetValue(cacheKey, out FlowerModel? memory) && memory is not null)
                return Response<FlowerModel>.Ok(memory);

            var res = await _flowerService.GetByIdAsync(id);
            if (res is null) return Response<FlowerModel>.Error(ErrorKeys.NotFound);
            _memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<FlowerModel>.Ok(res);
        }
    }
}
