using AsZero.DbContexts;
using FutureTech.Dal.Entities;
using FutureTech.Dal.Repository;

namespace ChangSha_Byd_NetCore8.AsZero
{
    public class AsZeroEfRepo<TKey, TEntity> : EfRepository<AsZeroDbContext, TKey, TEntity>
        where TEntity : GenericEntity<TKey>
    {
        public AsZeroEfRepo(AsZeroDbContext dbContext, IServiceProvider sp) : base(dbContext, sp)
        {
        }
    }
}
