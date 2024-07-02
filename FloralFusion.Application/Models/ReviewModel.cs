using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Application.Models
{
    public class ReviewModel
    {
        [MaxLength(200)]
        public required string Text { get; set; }

    }
}
