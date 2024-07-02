using FloralFusion.Domain.Data;
using FloralFusion.Domain.Entities;
using FloralFusion.Domain.Interfaces.Reports_and_Analytics;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories.ReportsAndAnalyticsRepositories
{
    public class SalesReportsRepos(FloralFusionDb floralFusionDb) :AbstractRepos<SalesReport>(floralFusionDb),ISalesReports
    {
        #region GenerateSalesReportAsync

        public async Task<SalesReport> GenerateSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            var order = await floralFusionDb.Orders.Where(i => i.OrderDate >= startDate && i.OrderDate <= endDate)
                            .ToListAsync()
                        ?? throw new ArgumentException("No order find between given data");
            var report = new SalesReport
            {
                TotalSales = order.Sum(i => i.TotalPrice),
                TotalOrders = order.Count,
                TotalRevenue = order.Sum(i => i.TotalPrice),
            };
            await dbset.AddAsync(report);
            await floralFusionDb.SaveChangesAsync();
            return report;
        }

        #endregion

        #region GetSalesReportsAsync

        public async Task<IEnumerable<SalesReport>> GetSalesReportsAsync()
        {
           return await dbset.ToListAsync();
        }

        #endregion
    }
}
