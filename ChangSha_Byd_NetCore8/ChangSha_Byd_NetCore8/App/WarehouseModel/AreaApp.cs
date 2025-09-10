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
    public class AreaApp : FutureBaseEntityService<int, Area>
    {
        private readonly IOptions<AppSetting> _appConfiguration;
        public IGenericRepository<int, Area> _areaRespository { get; }
        public AreaApp(IGenericRepository<int, Area> repo, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _areaRespository = repo;
            _appConfiguration = appConfiguration;
        }
        /// <returns></returns>
        public async Task<TableData> Load(QueryAreaReq request)
        {

            var query = new Specification<Area>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);

            var (count, data) = await LoadPageAsNoTrackingAsync(query, request.page, request.limit);

            return new TableData { count = count, data = data };
        }

        public async Task<IReadOnlyList<Area>> GetList(GetAreaListInput input)
        {

            var query = new Specification<Area>(u => !u.IsDeleted);
            if (!string.IsNullOrEmpty(input.Key))
            {
                query.CombineCritia(u => u.Code.Contains(input.Key) || u.Name.Contains(input.Key));
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var list = await Repository.Query(query).AsNoTracking().ToListAsync();

            return list;
        }

        public async Task<bool> IsHaveCode(string code)
        {
            var query = new Specification<Area>(a => !a.IsDeleted && a.Code == code);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var i = await Repository.Query(query).AsNoTracking().CountAsync();
            if (i > 0) { return true; }
            else { return false; }
        }
    }
}
