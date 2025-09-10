using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ChangSha_Byd_NetCore8.App.Production
{
    public class TempCodeApp : FutureBaseEntityService<int, TempCode>
    {
        public IGenericRepository<int, TempCode> _tempCodeRespository { get; }
        private readonly IOptions<AppSetting> _appConfiguration;
        public TempCodeApp(IGenericRepository<int, TempCode> repo, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _tempCodeRespository = repo;
            _appConfiguration = appConfiguration;
        }

        /// <summary>
        /// 获取新的任务号
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetNewCode()
        {
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            var query = new Specification<TempCode>(u => !u.IsDeleted);
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var entity = await Repository.Query(query).FirstOrDefaultAsync();
            if (entity != null)
            {
                //不能超过32767
                if (entity.Code >= 32700)
                {
                    entity.Code = 1;
                }
                else
                {
                    entity.Code = entity.Code + 1;
                }
                await Repository.UpdateAsync(entity, true);
                await Repository.SaveChangesAsync();
                return entity.Code;

            }
            else
            {

                return 0;
            }
        }
    }
}
