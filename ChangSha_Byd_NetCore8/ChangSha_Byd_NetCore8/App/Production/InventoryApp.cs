using AsZero.DbContexts;
using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Dynamic;

namespace ChangSha_Byd_NetCore8.App.Production
{
    public class InventoryApp : FutureBaseEntityService<int, Inventory>
    {
        private readonly IOptions<AppSetting> _appConfiguration;
        public readonly AsZeroDbContext _dBContext;
        public readonly IGenericRepository<int, Inventory> _repo;
        public InventoryApp(IGenericRepository<int, Inventory> repo, AsZeroDbContext dBContext, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _dBContext = dBContext;
            _appConfiguration = appConfiguration;
            _repo = repo;

        }
        public async Task<TableData> Load(QueryInventoryReq request)
        {

            var query = new Specification<Inventory>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.CarTypeNum.Contains(request.key));
            }
            if (request.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == request.AreaId);
            }

            if (request.LocationId != null)
            {
                query.CombineCritia(u => u.LocationId == request.LocationId);
            }
            if (request.CarTypeId != null)
            {
                query.CombineCritia(u => u.CarTypeId == request.CarTypeId);
            }
            if (!string.IsNullOrEmpty(request.carTypeNameKey))
            {
                query.CombineCritia(u => u.CarTypeName.Contains(request.carTypeNameKey));
            }
            var pageSpec = query.New().ApplyOrderByDescending(a => a.CreateTime).ApplyPaging(new Pagination(request.page, request.limit));

            var (count, data) = await LoadPageAsNoTrackingAsync(query, pageSpec);
            return new TableData { count = count, data = data };

        }

        public async Task<IReadOnlyList<Inventory>> GetByQuery(GetInventoryListInput input)
        {
            var query = new Specification<Inventory>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }
            if (input.LocationId != null)
            {
                query.CombineCritia(u => u.LocationId == input.LocationId);
            }
            if (input.CarTypeId != null)
            {
                query.CombineCritia(u => u.CarTypeId == input.CarTypeId);
            }
            if (!string.IsNullOrEmpty(input.CarTypeNum))
            {
                query.CombineCritia(u => u.CarTypeNum == input.CarTypeNum);
            }
            if (input.Ids != null && input.Ids.Count > 0)
            {
                query.CombineCritia(u => input.Ids.Contains(u.Id));
            }
            if (!string.IsNullOrEmpty(input.Key))
            {
                query.CombineCritia(u =>u.CarTypeName.Contains(input.Key));

             }
            var list = await Repository.Query(query).AsNoTracking().ToListAsync();

            return list;
        }

        /// <summary>
        /// 查询库存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<GetInventoryListOutput>> GetInventoryList(GetInventoryListInput input)
        {
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            var query = _dBContext.Inventorys.Where(a => !a.IsDeleted && a.WarehouseId == currentWarehouseId);
            
            if (input.AreaId != null)
            {
                query = query.Where(a => a.AreaId == input.AreaId);
            }
            if (input.LocationId != null)
            {
                query = query.Where(a => a.LocationId == input.LocationId);
            }
            if (input.CarTypeId != null)
            {
                query = query.Where(a => a.CarTypeId == input.CarTypeId);
            }
            if (!string.IsNullOrEmpty(input.CarTypeNum))
            {
                query = query.Where(a => a.CarTypeNum == input.CarTypeNum);
            }
            if (!string.IsNullOrEmpty(input.Key))
            {
                query = query.Where(a => a.CarTypeName.Contains(input.Key));
            }

            var query2 = _dBContext.Area_CarType_GateWays.Where(u => !u.IsDeleted && u.WarehouseId == currentWarehouseId);
            if (input.InGatewayId != null)
            {
                query2 = query2.Where(u => u.InGatewayId == input.InGatewayId);
            }
            if (input.OutGatewayId != null)
            {
                query2 = query2.Where(u => u.OutGatewayId == input.OutGatewayId);
            }
            if (input.IsRepair != null)
            {
                query2 = query2.Where(u => u.IsRepair == input.IsRepair);
            }
            query2 = query2.Include(u => u.InGateway).Include(u => u.OutGateway);

            return await (from a in query
                          join b in query2 on new { a.AreaId, a.CarTypeId } equals new { b.AreaId, b.CarTypeId }
                          into temp
                          from c in temp.DefaultIfEmpty()
                          select new GetInventoryListOutput
                          {
                              InventoryId = a.Id,
                              WarehouseId = a.WarehouseId,
                              AreaId = a.AreaId,
                              AreaName = a.AreaName,
                              LocationId = a.LocationId,
                              LocationCode = a.LocationCode,
                              CarTypeId = (int)a.CarTypeId,
                              CarTypeName = a.CarTypeName,
                              CarTypeNum = a.CarTypeNum,
                              CreateTime = a.CreateTime,
                              Status = a.Status,
                              SetTaskType = a.SetTaskType,
                              InGatewayId = c != null ? c.InGatewayId : 0,
                              InGatewayName = c != null ? c.InGateway.Name : "",
                              OutGatewayId = c != null ? c.OutGatewayId : 0,
                              OutGatewayName = c != null ? c.OutGateway.Name : "",

                          }).ToListAsync();

        }
        
        /// <summary>
        /// 查询立库某一个列的库存情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<GetBoardCeOutput>> GetBoardCe(GetBoardCeInput input)
        {
            List<GetBoardCeOutput> result = new List<GetBoardCeOutput>();
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            List<Location> locationList = null;
            if (currentWarehouseId == 1)
            {
                locationList = await _dBContext.Locations.Where(u => !u.IsDeleted && u.WarehouseId == currentWarehouseId &&  u.LineNo == input.Line && u.ColumnNo == input.Column).AsNoTracking().ToListAsync();
            }
            else
            {
                locationList = await _dBContext.Locations.Where(u => !u.IsDeleted && u.WarehouseId == currentWarehouseId && u.AreaId == input.AreaId && u.LineNo == input.Line && u.ColumnNo == input.Column).AsNoTracking().ToListAsync();
            }
            if (locationList != null)
            {
                var locationIds = locationList.Select(a => a.Id).ToArray();

                var query = new Specification<Inventory>(u => !u.IsDeleted && u.WarehouseId == currentWarehouseId && locationIds.Contains(u.LocationId));
                var list = await Repository.Query(query).AsNoTracking().ToListAsync();

                foreach (var item in locationList)
                {
                    var entity = list.Where(a => a.LocationId == item.Id).FirstOrDefault();
                    GetBoardCeOutput output = new GetBoardCeOutput();
                    output.LocationId = item.Id;
                    output.FloorNo = item.FloorNo;
                    output.LocationCode = item.Code;
                    if (entity != null)
                    {
                        output.CarTypeName = entity.CarTypeName;
                        output.CarTypeNum = entity.CarTypeNum;
                        output.SetTaskType = entity.SetTaskType;
                        output.CreateTime = entity.CreateTime;
                    }
                    result.Add(output);
                }
                result = result.OrderByDescending(a => a.FloorNo).ToList();
            }
            return result;
        }
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Inventory> GetInventoryEntity(GetInventoryListInput input)
        {
            var query = new Specification<Inventory>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }
            if (input.NotAreaId != null)
            {
                query.CombineCritia(u => u.AreaId != input.AreaId);
            }
            if (input.LocationId != null)
            {
                query.CombineCritia(u => u.LocationId == input.LocationId);
            }
            if (input.CarTypeId != null)
            {
                query.CombineCritia(u => u.CarTypeId == input.CarTypeId);
            }
            if (!string.IsNullOrEmpty(input.CarTypeNum))
            {
                query.CombineCritia(u => u.CarTypeNum == input.CarTypeNum);
            }
            if (input.CarTypeFace!=null)
            {
                query.CombineCritia(u => u.CarTypeFace == input.CarTypeFace);
            }
            if (input.Ids != null && input.Ids.Count > 0)
            {
                query.CombineCritia(u => input.Ids.Contains(u.Id));
            }
            if (input.CarTypeInt != null)
            {
                query.CombineCritia(u => u.CarTypeInt == input.CarTypeInt);
            }
            if (input.Status != null)
            {
                query.CombineCritia(u => u.Status == input.Status);
            }
            return await Repository.Query(query).AsNoTracking().FirstOrDefaultAsync();

         


        }

        // 获取库存列表
        //这个carTypeList需要用到apenAuth,但是我感觉可以省略掉这一步直接获取
        public async Task<List<GetInventoryReportOutput>> GetInventoryReport()
        {
            List<GetInventoryReportOutput> result = new List<GetInventoryReportOutput>();
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;

            var query = _dBContext.Inventorys.Where(u => !u.IsDeleted && u.WarehouseId == currentWarehouseId);

            if (currentWarehouseId == 1)//主线ub库
            {
                result = await query
                    .GroupBy(a => a.CarTypeName)
                   .Select(a => new GetInventoryReportOutput()
                   {
                       CarTypeName = a.Key,
                       //CarTypeName = a.Select(n => n.CarTypeName).FirstOrDefault()??"",
                       Count = a.Count()
                   }).ToListAsync();
            }
            else
            {

                //var carTypeList = await _openAuthDBContext.Categories.Where(a => !a.IsDeleted && a.TypeId == "CarType").AsNoTracking().ToListAsync();

                var carTypeList = new List<Category>
                {
                    new Category
                    {
                        Id = "b8611edd-ecee-4b51-928b-9df0bfe5be82",
                        Name = "EKEA",
                        DtCode = "",
                        DtValue = "2",
                        Enable = false,
                        SortNo = 0,
                        Description = "",
                        TypeId = "CarType",
                        CreateTime = DateTime.Parse("2022-08-17 10:06:09.5466610"),
                        CreateUserId = "49df1602-f5f3-4d52-afb7-3802da619558",
                        CreateUserName = "管理员",
                        UpdateTime = DateTime.Parse("2022-08-17 10:06:09.5466554"),
                        UpdateUserId = null,
                        UpdateUserName = null,
                        IsDeleted = false
                    },
                    new Category
                    {
                        Id = "c33ad4b8-301f-4e74-a6e2-a8c3075ad6cf",
                        Name = "SC2E",
                        DtCode = "",
                        DtValue = "1",
                        Enable = false,
                        SortNo = 0,
                        Description = "",
                        TypeId = "CarType",
                        CreateTime = DateTime.Parse("2022-08-17 10:05:45.4187092"),
                        CreateUserId = "49df1602-f5f3-4d52-afb7-3802da619558",
                        CreateUserName = "管理员",
                        UpdateTime = DateTime.Parse("2022-08-17 10:05:45.4172853"),
                        UpdateUserId = null,
                        UpdateUserName = null,
                        IsDeleted = false
                    }
                };


                var a = from s in query
                        group s by s.CarTypeInt into temp
                        select new { CarTypeInt = temp.Key, Count = temp.Count() };

                var list = await a.AsNoTracking().ToListAsync();
                foreach (var i in list)
                {
                    GetInventoryReportOutput output = new GetInventoryReportOutput()
                    {
                        CarTypeName = carTypeList.Where(d => d.DtValue == i.CarTypeInt.ToString()).Select(d => d.Name).FirstOrDefault(),
                        Count = i.Count
                    };
                    result.Add(output);
                }


                //result = await query
                //    .GroupBy(a => a.CarTypeName)
                //   .Select(a => new GetInventoryReportOutput()
                //   {
                //       CarTypeName = a.Key,
                //        //CarTypeName = a.Select(n => n.CarTypeName).FirstOrDefault()??"",
                //        Count = a.Count()
                //   }).ToListAsync();



            }

            return result;
        }

        public class Category
        {

            public string Id { get; set; }

            public bool IsDeleted { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            [Description("名称")]
            public string Name { get; set; }
            /// <summary>
            /// 代码
            /// </summary>
            [Description("代码")]
            public string DtCode { get; set; }
            /// <summary>
            /// 通常与字典代码标识一致，但万一有不一样的情况呢？
            /// </summary>
            [Description("值")]
            public string DtValue { get; set; }
            /// <summary>
            /// 是否可用
            /// </summary>
            [Description("是否可用")]
            public bool Enable { get; set; }
            /// <summary>
            /// 排序号
            /// </summary>
            [Description("排序号")]
            public int SortNo { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            [Description("描述")]
            public string Description { get; set; }
            /// <summary>
            /// 分类ID
            /// </summary>
            [Description("分类标识")]
            public string TypeId { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            [Description("创建时间")]
            public System.DateTime CreateTime { get; set; }
            /// <summary>
            /// 创建人ID
            /// </summary>
            [Description("创建人ID")]
            [Browsable(false)]
            public string CreateUserId { get; set; }
            /// <summary>
            /// 创建人
            /// </summary>
            [Description("创建人")]
            public string CreateUserName { get; set; }
            /// <summary>
            /// 最后更新时间
            /// </summary>
            [Description("最后更新时间")]
            public System.DateTime? UpdateTime { get; set; }
            /// <summary>
            /// 最后更新人ID
            /// </summary>
            [Description("最后更新人ID")]
            [Browsable(false)]
            public string UpdateUserId { get; set; }
            /// <summary>
            /// 最后更新人
            /// </summary>
            [Description("最后更新人")]
            public string UpdateUserName { get; set; }
        }
    }
}
