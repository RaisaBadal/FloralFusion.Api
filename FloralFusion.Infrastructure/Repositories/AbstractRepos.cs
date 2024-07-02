using FloralFusion.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace FloralFusion.Infrastructure.Repositories
{
    public class AbstractRepos<T>(FloralFusionDb floralFusionDb)
        where T : class
    {
        protected readonly FloralFusionDb floralFusionDb = floralFusionDb;
        protected DbSet<T> dbset = floralFusionDb.Set<T>();
    }
}
