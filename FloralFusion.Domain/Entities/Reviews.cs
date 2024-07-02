using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Domain.Entities
{
    public class Reviews: AbstractEntity
    {
        public string UserId { get; set; }

        public long FlowerId { get; set; }

        [MaxLength(200)]
        public required string Text { get; set; }

        public DateTime CreatedAt { get; set; }=DateTime.Now;

        public DateTime UpdateAt { get; set; }=DateTime.Now;

        public bool IsActive { get; set; } = true;

        public User User { get; set; }

        public Flower Flower { get; set; }
    }
}
