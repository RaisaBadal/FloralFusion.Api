namespace FloralFusion.Application.Models
{
    public class PaymentModel
    {
        public required string MethodName { get; set; }

        public required string Description { get; set; }

        public decimal? FeePercentage { get; set; }

        public decimal? MinAmount { get; set; }

        public decimal? MaxAmount { get; set; }

    }
}
