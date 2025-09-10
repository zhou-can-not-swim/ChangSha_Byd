using AsZero.DbContexts;
using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.EntityFrameworkCore;

namespace ChangSha_Byd_NetCore8.App.WarehouseModel
{

    public class WarehouseApp : FutureBaseEntityService<int, Warehouse>
    {
        public readonly AsZeroDbContext _dBContext;
        public readonly AreaApp _areaApp;
        public readonly LocationApp _locationApp;
        public WarehouseApp(IGenericRepository<int, Warehouse> repo, AreaApp areaApp, AsZeroDbContext dBContext, LocationApp locationApp) : base(repo)
        {
            _dBContext = dBContext;
            _areaApp = areaApp;
            _locationApp = locationApp;
        }

        /// <summary>
        /// 分页加载列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TableData> Load(QueryWarehouseReq request)
        {

            var query = new Specification<Warehouse>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            var (count, data) = await LoadPageAsNoTrackingAsync(query, request.page, request.limit);

            return new TableData { count = count, data = data };
        }

        public async Task<IReadOnlyList<Warehouse>> GetList(QueryWarehouseReq request)
        {

            var query = new Specification<Warehouse>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            var list = await ListAsync(query);

            return list;
        }

        public async Task<bool> IsHaveCode(string code)
        {
            var query = new Specification<Warehouse>(a => !a.IsDeleted && a.Code == code);
            var i = await Repository.Query(query).AsNoTracking().CountAsync();
            if (i > 0) { return true; }
            else { return false; }
        }
        public async Task<Warehouse> AddWarehouse(WarehouseDto input)
        {

            var res = await this.AddAsync(input.Warehouse, false);
            if (input.DetailList.Count > 0)
            {
                foreach (var item in input.DetailList)
                {
                    item.Warehouse = null;
                    item.WarehouseId = res.Data.Id;
                    await _areaApp.AddRangeAsync(input.DetailList, false);
                }

            }


            //全部提交
            await Repository.SaveChangesAsync();

            return res.Data;
        }
        public async Task<Warehouse> UpdateWarehouse(WarehouseDto input)
        {

            var res = await this.UpdateForNotTrackedAsync(input.Warehouse, false);
            //先获取原来已有的字表信息
            GetAreaListInput getAreaListInput = new GetAreaListInput() { WarehouseId = input.Warehouse.Id };
            var detailList = await _areaApp.GetList(getAreaListInput);
            var oldIds = detailList.Select(s => s.Id);
            var newIds = input.DetailList.Where(s => s.Id != 0).ToList().Select(s => s.Id);
            //找差集,差集需要删掉
            var chaIds = oldIds.Except(newIds);
            await _areaApp.DeleteRangeForNotTrackedAsync(chaIds.ToArray(), false);

            var updateDetails = input.DetailList.Where(a => a.Id != 0).ToList();
            await _areaApp.UpdateRangeForTrackedAsync(updateDetails, false);

            var addDetails = input.DetailList.Where(a => a.Id == 0).ToList();
            await _areaApp.AddRangeAsync(addDetails, false);


            //全部提交
            await Repository.SaveChangesAsync();

            return res.Data;
        }



    }
}
