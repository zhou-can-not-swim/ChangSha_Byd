using FutureTech.Dal;
using FutureTech.Dal.Repository;

namespace ChangSha_Byd_NetCore8.AsZero
{
    public static class AsZeroRepoServiceCollection
    {
        public static IServiceCollection AddAsZeroRepositories(this IServiceCollection services)
        {
            services.AddDal(o => {
                o.EnableOpsHisotry = false;
            });
            services.AddScoped(typeof(IGenericRepository<,>), typeof(AsZeroEfRepo<,>));//将泛型仓储接口 IGenericRepository<,>
                                                                                       //映射到具体实现 AsZeroEfRepo<,>
            return services;
        }
    }
}
