using System.ComponentModel.DataAnnotations;

namespace FloralFusion.Domain.Entities
{
    public class AbstractEntity
    {
        [Key]
        public long Id { get; set; }
      
    }
}
