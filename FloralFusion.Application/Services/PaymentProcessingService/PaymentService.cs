using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Custom_Exceptions;
using FloralFusion.Application.Interfaces.PaymentMethods;
using FloralFusion.Application.Models;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Persistanse.OuterServices;

namespace FloralFusion.Application.Services.PaymentProcessingService
{
    public class PaymentService : AbstractClass, IPaymentService
    {
        public PaymentService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService) : base(uniteOfWork, mapper, smtpService)
        {
        }

        #region CreateAsync
        public async Task<long> CreateAsync(PaymentModel entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            var payment=await uniteOfWork.PaymentMethods.GetAllAsync();

            if (!payment.Any()) throw new GeneralException(ErrorKeys.NotFound);
            if (payment.FirstOrDefault(i => i.MethodName == entity.MethodName) is not null)
                throw new InvalidOperationException(ErrorKeys.BadRequest);

            var mapped=mapper.Map<PaymentMethod>(entity)
                  ?? throw new InvalidOperationException(ErrorKeys.Mapped);

            var res = await uniteOfWork.PaymentMethods.Create(mapped);
            return res;
        }
        #endregion

        #region DeleteByIdAsync
        public async Task DeleteByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            await uniteOfWork.PaymentMethods.DeleteByIdAsync(id);
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<PaymentModel>> GetAllAsync()
        {
            var res = await uniteOfWork.PaymentMethods.GetAllAsync()
                ?? throw new InvalidOperationException(ErrorKeys.NotFound);
            var mapped=mapper.Map<IEnumerable<PaymentModel>>(res)
                  ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion

        #region GetByIdAsync

        public async Task<PaymentModel> GetByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var res = await uniteOfWork.PaymentMethods.GetByIdAsync(id)
                ?? throw new InvalidOperationException(ErrorKeys.NotFound);
            var mapped=mapper.Map<PaymentModel>(res)
                   ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion

        #region SoftDeleteByIdAsync

        public async Task<bool> SoftDeleteByIdAsync(long id)
        {
            if (id < 0) throw new ArgumentException($"Invalid id: {id}");
            var res = await uniteOfWork.PaymentMethods.SoftDeleteByIdAsync(id);
            return res;
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(long id, PaymentModel entity)
        {
            if (id < 0) throw new ArgumentException("Invalid id");
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            var mapped = mapper.Map<PaymentMethod>(entity)
                ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            var res = await uniteOfWork.PaymentMethods.Update(id, mapped);
            return res;
        }
        #endregion
    }
}
