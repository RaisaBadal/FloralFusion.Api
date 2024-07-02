using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Domain.Entities
{
    [Table("SalesReports")]
    public class SalesReport:AbstractEntity
    {
        public DateTime ReportDate { get; set; } = DateTime.Now;

        public decimal TotalSales { get; set; }

        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }

        //public List<Order> Orders { get; set; }

        //public long OrderId { get; set; }
    }
}
