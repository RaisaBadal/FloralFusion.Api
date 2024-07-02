namespace FloralFusion.Domain.Interfaces.Admin
{
    public interface IManageFlowers
    {
        Task<bool> AddFlowerCategoryAsync(string name);
        Task<bool> AddFlowerOccasionAsync(string name);
    }
}
