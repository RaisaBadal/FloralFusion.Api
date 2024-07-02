namespace FloralFusion.Domain.Entities
{
    public class SearchHistory:AbstractEntity
    {
        public string UserId { get; set; }
        public string SearchQuery { get; set; }
        public DateTime SearchedAt { get; set; }
        public User User { get; set; }
    }
}
