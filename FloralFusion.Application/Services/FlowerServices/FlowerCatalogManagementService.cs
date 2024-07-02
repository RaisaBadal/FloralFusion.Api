using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.Flowers;
using FloralFusion.Application.Models;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;

namespace FloralFusion.Application.Services.FlowerServices
{
    public class FlowerCatalogManagementService : AbstractClass, IFlowerCatalogManagementService
    {
        public FlowerCatalogManagementService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService) : base(uniteOfWork, mapper, smtpService)
        {
        }

        //funqcionals gamoiyenebs seller

        #region Create
        public async Task<long> Create(FlowerModel entity, string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));

                var mapped = mapper.Map<Flower>(entity)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                mapped.UserId= userId;

                var flower = await uniteOfWork.FlowerRepository.GetFlowersByName(entity.Name)
                    ?? throw new GeneralException(ErrorKeys.NotFound);

                var userFlower = flower.FirstOrDefault(i => i.Name == entity.Name
                && i.CategoryId == entity.CategoryId
                && i.OccasionId == entity.OccasionId
                && i.Color == entity.Color
                && i.UserId == userId);

                if(userFlower is not null)
                {
                    userFlower.Quantity += entity.Quantity;
                    await uniteOfWork.SaveChanges();
                    return userFlower.Id;
                }

                var res=await uniteOfWork.FlowerCatalogManagement.Create(mapped);
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
                ArgumentNullException.ThrowIfNull(userId,nameof(userId));
                var res=await uniteOfWork.FlowerCatalogManagement.GetByIdAsync(id);
                if (res.UserId != userId) throw new GeneralException(ErrorKeys.BadRequest);
                await uniteOfWork.FlowerCatalogManagement.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<FlowerModel>> GetAllAsync(string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var res = await uniteOfWork.FlowerCatalogManagement.GetAllAsync();
                var userAllFlower=res.Where(i=>i.UserId==userId).ToList();
                if(!userAllFlower.Any()) throw new GeneralException(ErrorKeys.NotFound);
                var mapped=mapper.Map<IEnumerable<FlowerModel>>(userAllFlower)
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
        public async Task<FlowerModel> GetByIdAsync(long id, string userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var res = await uniteOfWork.FlowerCatalogManagement.GetByIdAsync(id);
                if(res.UserId!=userId) throw new ArgumentException($"{userId} not have flower with id: {id}");
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

        #region SoftDeleteByIdAsync
        public async Task<bool> SoftDeleteByIdAsync(long id, string userId)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var fl = await uniteOfWork.FlowerCatalogManagement.GetByIdAsync(id);
                if (fl.UserId != userId) throw new GeneralException(ErrorKeys.BadRequest);
                var res=await uniteOfWork.FlowerCatalogManagement.SoftDeleteByIdAsync(id);
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
        public async Task<bool> Update(long id, FlowerModel entity, string userId)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.ArgumentNull);
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var mapped = mapper.Map<Flower>(entity)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                mapped.UserId = userId;
                var res=await uniteOfWork.FlowerCatalogManagement.Update(id, mapped);
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
