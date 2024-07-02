using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.DeliveryOptions;
using FloralFusion.Application.Models;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;

namespace FloralFusion.Application.Services.DeliveryManagementServices
{
    public class DeliveryOptionService : AbstractClass, IDeliveryOptionService
    {
        public DeliveryOptionService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService) : base(uniteOfWork, mapper, smtpService)
        {
        }

        #region Create
        public async Task<long> CreateAsync(DeliveryOptionModel entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                var mapped = mapper.Map<DeliveryOption>(entity)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                var res = await uniteOfWork.DeliveryOptions.Create(mapped);
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message,ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region DeleteByIdAsync
        public async Task DeleteByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                await uniteOfWork.DeliveryOptions.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<DeliveryOptionModel>> GetAllAsync()
        {
            try
            {
                var res = await uniteOfWork.DeliveryOptions.GetAllAsync();
                var mapped = mapper.Map<IEnumerable<DeliveryOptionModel>>(res)
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
        public async Task<DeliveryOptionModel> GetByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var res = await uniteOfWork.DeliveryOptions.GetByIdAsync(id)
                    ?? throw new GeneralException(ErrorKeys.BadRequest);
                var mapped = mapper.Map<DeliveryOptionModel>(res)
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
        public async Task<bool> SoftDeleteByIdAsync(long id)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                var res = await uniteOfWork.DeliveryOptions.SoftDeleteByIdAsync(id);
                return res;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }
        #endregion

        #region Update
        public async Task<bool> UpdateAsync(long id, DeliveryOptionModel entity)
        {
            try
            {
                if (id < 0) throw new ArgumentException(ErrorKeys.BadRequest);
                ArgumentNullException.ThrowIfNull(entity, nameof(entity));
                var mapped = mapper.Map<DeliveryOption>(entity)
                    ?? throw new GeneralException(ErrorKeys.Mapped);
                var res = await uniteOfWork.DeliveryOptions.Update(id, mapped);
                return res;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
