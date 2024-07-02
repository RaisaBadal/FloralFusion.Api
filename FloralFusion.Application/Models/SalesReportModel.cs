namespace FloralFusion.Application.Models
{
    public class SalesReportModel
    {
        public DateTime ReportDate { get; set; } 

        public decimal TotalSales { get; set; }

        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
