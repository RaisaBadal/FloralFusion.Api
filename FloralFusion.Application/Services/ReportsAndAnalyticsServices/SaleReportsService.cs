using AutoMapper;
using BookBridge.Application.StaticFiles;
using FloralFusion.Application.Models;
using FloralFusion.Domain.Interfaces;
using FloralFusion.Domain.Interfaces.ReportsAnsAnalytisctsService;
using FloralFusion.Persistanse.OuterServices;

namespace FloralFusion.Application.Services.ReportsAndAnalyticsServices
{
    public class SaleReportsService : AbstractClass, ISalesReportsService
    {
        public SaleReportsService(IUniteOfWork uniteOfWork, IMapper mapper, SmtpService smtpService) : base(uniteOfWork, mapper, smtpService)
        {
        }

        #region GenerateSalesReportAsync
        public async Task<SalesReportModel> GenerateSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be greater than end date.");
            }
            if (startDate > DateTime.Now || endDate > DateTime.Now)
            {
                throw new ArgumentException("Date range cannot be in the future.");
            }

            var res = await uniteOfWork.SalesReports.GenerateSalesReportAsync(startDate, endDate)
                ?? throw new InvalidOperationException(ErrorKeys.NotFound);
            var mapped=mapper.Map<SalesReportModel>(res)
                  ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;
        }
        #endregion

        #region GetSalesReportsAsync

        public async Task<IEnumerable<SalesReportModel>> GetSalesReportsAsync()
        {
            var res = await uniteOfWork.SalesReports.GetSalesReportsAsync()
                ?? throw new InvalidOperationException(ErrorKeys.NotFound);
            var mapped=mapper.Map<IEnumerable<SalesReportModel>>(res)
                 ?? throw new InvalidOperationException(ErrorKeys.Mapped);
            return mapped;

        }
        #endregion
    }
}
