using FloralFusion.Domain.Entities;

namespace FloralFusion.Domain.Interfaces.Reports_and_Analytics
{
    public interface ISalesReports
    {
        Task<SalesReport> GenerateSalesReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<SalesReport>> GetSalesReportsAsync();
    }
}
