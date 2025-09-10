using Byd.Services.Request;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace ChangSha_Byd_NetCore8.App.WarehouseModel
{
    public class EquipmentApp : FutureBaseEntityService<int, Equipment>
    {
        private readonly IOptions<AppSetting> _appConfiguration;
        public EquipmentApp(IGenericRepository<int, Equipment> repo, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _appConfiguration = appConfiguration;
        }

        /// <summary>
        /// 分页加载列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TableData> Load(QueryEquipmentReq request)
        {

            var query = new Specification<Equipment>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);

            var (count, data) = await LoadPageAsNoTrackingAsync(query, request.page, request.limit);

            return new TableData { count = count, data = data };
        }

        public async Task<IReadOnlyList<Equipment>> GetList(EquipmentRequest request)
        {

            var query = new Specification<Equipment>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var list = await Repository.Query(query).AsNoTracking().ToListAsync();

            return list;
        }
        public async Task<bool> IsHaveCode(string code)
        {
            var query = new Specification<Equipment>(a => !a.IsDeleted && a.Code == code);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var i = await Repository.Query(query).AsNoTracking().CountAsync();
            if (i > 0) { return true; }
            else { return false; }
        }

    }
}
