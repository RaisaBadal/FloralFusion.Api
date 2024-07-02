using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.Flowers;
using FloralFusion.Application.Models;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;

namespace FloralFusion.Application.Services.FlowerServices
{
    public class FlowersService : AbstractClass, IFlowerService
    {
        public FlowersService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService) : base(uniteOfWork, mapper, smtpService)
        {
        }

        #region GetAllAsync
        public async Task<IEnumerable<FlowerModel>> GetAllAsync()
        {
            try
            {
                var res = await uniteOfWork.FlowerCatalogManagement.GetAllAsync();
                if (!res.Any()) throw new GeneralException(ErrorKeys.BadRequest);
                var mapped = mapper.Map<IEnumerable<FlowerModel>>(res)
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

        #region GetBetweenPrice

        public async Task<IEnumerable<FlowerModel>> GetBetweenPrice(decimal price1, decimal price2)
        {
            try
            {
                if (price1 > price2 || price1 < 0 || price2 < 0) throw new GeneralException(ErrorKeys.BadRequest);
                var res = await uniteOfWork.FlowerRepository.GetBetweenPrice(price1, price2);
                if (!res.Any()) throw new GeneralException(ErrorKeys.BadRequest);

                var mapped = mapper.Map<IEnumerable<FlowerModel>>(res)
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

        #region GetByCategory

        public async Task<IEnumerable<FlowerModel>> GetByCategory(string category)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(category, nameof(category));
                var res = await uniteOfWork.FlowerRepository.GetByCategory(category);
                if (!res.Any()) throw new GeneralException(ErrorKeys.BadRequest);

                var mapped = mapper.Map<IEnumerable<FlowerModel>>(res)
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

        #region GetByIdAsync

        public async Task<FlowerModel> GetByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                var res = await uniteOfWork.FlowerCatalogManagement.GetByIdAsync(id)
                    ?? throw new GeneralException(ErrorKeys.BadRequest);
                var mapped = mapper.Map<FlowerModel>(res)
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

        #region GetByOccasion
        public async Task<IEnumerable<FlowerModel>> GetByOccasion(string occasion)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(occasion, nameof(occasion));
                var res = await uniteOfWork.FlowerRepository.GetByOccasion(occasion);
                if (!res.Any()) throw new GeneralException(ErrorKeys.BadRequest);

                var mapped = mapper.Map<IEnumerable<FlowerModel>>(res)
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

        #region GetCategoryBetweenPrice
        public async Task<IEnumerable<FlowerModel>> GetCategoryBetweenPrice(string category, decimal price1, decimal price2)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(category, nameof(category));
                if (price1 > price2 || price1 < 0 || price2 < 0) throw new GeneralException(ErrorKeys.BadRequest);
                var res = await uniteOfWork.FlowerRepository.GetCategoryBetweenPrice(category, price1, price2);
                if (!res.Any()) throw new GeneralException(ErrorKeys.BadRequest);

                var mapped = mapper.Map<IEnumerable<FlowerModel>>(res)
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

        #region GetFeatured
        public async Task<IEnumerable<FlowerModel>> GetFeatured()
        {
            try
            {
                var res = await uniteOfWork.FlowerRepository.GetFeatured();
                if(!res.Any()) throw new GeneralException(ErrorKeys.BadRequest);
                var mapped = mapper.Map<IEnumerable<FlowerModel>>(res)
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

        #region GetFlowersByColor
        public async Task<IEnumerable<FlowerModel>> GetFlowersByColor(string color)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(color, nameof(color));
                var res = await uniteOfWork.FlowerRepository.GetFlowersByColor(color);
                if (!res.Any()) throw new GeneralException(ErrorKeys.BadRequest);

                var mapped = mapper.Map<IEnumerable<FlowerModel>>(res)
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

        #region GetFlowersByName
        public async Task<IEnumerable<FlowerModel>> GetFlowersByName(string name)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(name, nameof(name));
                var res = await uniteOfWork.FlowerRepository.GetFlowersByName(name);
                if (!res.Any()) throw new GeneralException(ErrorKeys.BadRequest);

                var mapped = mapper.Map<IEnumerable<FlowerModel>>(res)
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
    }
}
