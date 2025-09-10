using Byd.Services.Request;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using ChangSha_Byd_NetCore8.Protocols.Common;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChangSha_Byd_NetCore8.App.WarehouseModel
{
    public class GatewayApp : FutureBaseEntityService<int, Gateway>
    {
        private readonly IOptions<AppSetting> _appConfiguration;
        public GatewayApp(IGenericRepository<int, Gateway> repo, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _appConfiguration = appConfiguration;
        }

        /// <summary>
        /// 分页加载列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TableData> Load(QueryGatewayReq request)
        {

            var query = new Specification<Gateway>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (request.EquipmentId != null)
            {
                query.CombineCritia(u => u.EquipmentId == request.EquipmentId);
            }

            var pageSpec = query.New().AddInclude(u => u.Warehouse).AddInclude(u => u.Equipment).ApplyOrderBy(a => a.Id).ApplyPaging(new Pagination(request.page, request.limit));

            var (count, data) = await LoadPageAsNoTrackingAsync(query, pageSpec);
            return new TableData { count = count, data = data };

        }


        public async Task<IReadOnlyList<Gateway>> GetList(GetGateWayListInput request)
        {

            var query = new Specification<Gateway>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (request.EquipmentId != null)
            {
                query.CombineCritia(u => u.EquipmentId == request.EquipmentId);
            }
            if (request.Type != null)
            {
                query.CombineCritia(u => u.Type == (GatewayType)request.Type);
            }
            if (request.LocationId != null)
            {
                query.CombineCritia(u => u.LocationId == request.LocationId);
            }
            var list = await Repository.Query(query.AddInclude(a => a.Warehouse).AddInclude(a => a.Equipment)).AsNoTracking().ToListAsync();


            return list;
        }


        public async Task<bool> IsHaveCode(Gateway obj)
        {
            var query = new Specification<Gateway>(a => !a.IsDeleted && a.Code == obj.Code &&a.Type==obj.Type);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (obj.Id > 0)
            {
                query.CombineCritia(u => u.Id!=obj.Id);
            }
            var i = await Repository.Query(query).AsNoTracking().CountAsync();
            if (i > 0) { return true; }
            else { return false; }
        }

        public async Task<Gateway> GetGatewayEntity(GetGatewayEntityInput input)
        {
            var query = new Specification<Gateway>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (!string.IsNullOrEmpty(input.Code))
            {
                query.CombineCritia(u => u.Code == input.Code);
            }
            
            return await Repository.Query(query).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}