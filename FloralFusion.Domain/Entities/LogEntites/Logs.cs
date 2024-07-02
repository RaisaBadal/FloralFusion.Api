using System.ComponentModel.DataAnnotations.Schema;

namespace FloralFusion.Domain.Entities.LogEntites
{
    [Table(nameof(Logs))]
    public class Logs:AbstractEntity
    {
        public string? LogLevel { get; set; }

        public string? Message { get; set; }
    }
}
