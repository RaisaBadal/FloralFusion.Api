using FloralFusion.Application.Models;

namespace FloralFusion.Domain.Interfaces.ReportsAnsAnalytisctsService
{
    public interface ISalesReportsService
    {
        Task<SalesReportModel> GenerateSalesReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<SalesReportModel>> GetSalesReportsAsync();
    }
}
