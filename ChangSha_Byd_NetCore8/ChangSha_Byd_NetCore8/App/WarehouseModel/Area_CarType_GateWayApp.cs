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

    public class Area_CarType_GateWayApp : FutureBaseEntityService<int, Area_CarType_GateWay>
    {
        private readonly IOptions<AppSetting> _appConfiguration;
        public IGenericRepository<int, Area_CarType_GateWay> _area_CarType_GateWayRespository { get; }
        public Area_CarType_GateWayApp(IGenericRepository<int, Area_CarType_GateWay> repo, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _area_CarType_GateWayRespository = repo;
            _appConfiguration = appConfiguration;
        }
        public async Task<TableData> Load(QueryArea_CarType_GateWayReq input)
        {

            var query = new Specification<Area_CarType_GateWay>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }
            if (input.CarTypeId != null)
            {
                query.CombineCritia(u => u.CarTypeId == input.CarTypeId);
            }
            if (input.InGateWayId != null)
            {
                query.CombineCritia(u => u.InGatewayId == input.InGateWayId);
            }
            if (input.OutGateWayId != null)
            {
                query.CombineCritia(u => u.OutGatewayId == input.OutGateWayId);
            }
            var pageSpec = query.New().AddInclude(u => u.Area).AddInclude(u => u.CarType).AddInclude(u => u.InGateway).AddInclude(u => u.OutGateway).ApplyOrderByDescending(a => a.CarTypeId).ApplyPaging(new Pagination(input.page, input.limit));

            var (count, data) = await LoadPageAsNoTrackingAsync(query, pageSpec);
            return new TableData { count = count, data = data };
        }

                                                      //区域 车型 库口 list
        public async Task<List<Area_CarType_GateWay>> GetArea_CarType_GateWayList(GetArea_CarType_GateWayListInput input)
        {
            var query = new Specification<Area_CarType_GateWay>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }
            if (input.CarTypeId != null)//入库的时候应该只用了carTypeId（3）
            {
                query.CombineCritia(u => u.CarTypeId == input.CarTypeId);
            }
            if (input.InGateWayId != null)
            {
                query.CombineCritia(u => u.InGatewayId == input.InGateWayId);
            }
            if (input.OutGateWayId != null)
            {
                query.CombineCritia(u => u.OutGatewayId == input.OutGateWayId);
            }
            if (input.IsRepair != null)
            {
                query.CombineCritia(u => u.IsRepair == input.IsRepair);
            }

            //返回符合carTypeId的所有记录
            return await Repository.Query(query.AddInclude(a => a.Area).AddInclude(a => a.InGateway).AddInclude(a => a.OutGateway))
                .AsNoTracking().ToListAsync();
        }

        //区域 车型 库口  --> 一个entity
        public async Task<Area_CarType_GateWay> GetArea_CarType_GateWay(GetArea_CarType_GateWayInput input)
        {
            var query = new Specification<Area_CarType_GateWay>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (input.CarTypeId != null)
            {
                query.CombineCritia(u => u.CarTypeId == input.CarTypeId);
            }
            if (input.InGateWayId != null)
            {
                query.CombineCritia(u => u.InGatewayId == input.InGateWayId);
            }
            if (input.OutGateWayId != null)
            {
                query.CombineCritia(u => u.OutGatewayId == input.OutGateWayId);
            }
            return await Repository.Query(query
                .AddInclude(a => a.InGateway)
                .AddInclude(a => a.OutGateway))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
