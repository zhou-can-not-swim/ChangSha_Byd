using AsZero.DbContexts;
using Byd.Services.Request;
using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using ChangSha_Byd_NetCore8.Protocols.Common;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace ChangSha_Byd_NetCore8.App.Production
{

    public class StockTaskHistoryApp : FutureBaseEntityService<int, StockTaskHistory>
    {
        private readonly IOptions<AppSetting> _appConfiguration;
        private readonly AsZeroDbContext _dbContext;
        public StockTaskHistoryApp(IGenericRepository<int, StockTaskHistory> repo, AsZeroDbContext dbContext, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _dbContext = dbContext;
            _appConfiguration = appConfiguration;
        }

        #region 没用到history
        //public async Task<TableData> Load(QueryStockTaskHistoryReq request)
        //{

        //    var query = new Specification<StockTaskHistory>(u => !u.IsDeleted);
        //    var currentWarehouseId = _appConfiguration.Value.WarehouseId;
        //    query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
        //    if (!string.IsNullOrEmpty(request.key))
        //    {
        //        query.CombineCritia(u => u.CarTypeNum.Contains(request.key));
        //    }
        //    if (request.AreaId != null)
        //    {
        //        query.CombineCritia(u => u.AreaId == request.AreaId);
        //    }
        //    if (request.Type != null)
        //    {
        //        query.CombineCritia(u => (int)u.TaskType == request.Type);
        //    }
        //    if (request.Status != null)
        //    {
        //        query.CombineCritia(u => (int)u.TaskStatus == request.Status);
        //    }
        //    if (request.NotStatus != null)
        //    {
        //        query.CombineCritia(u => (int)u.TaskStatus != request.NotStatus);
        //    }
        //    if (request.StartTime != null)
        //    {
        //        query.CombineCritia(u => u.CompleteTime >= request.StartTime);
        //    }
        //    if (request.EndTime != null)
        //    {
        //        query.CombineCritia(u => u.CompleteTime < ((DateTime)request.EndTime).AddDays(1));
        //    }
        //    if (!string.IsNullOrEmpty(request.CarTypeNameKey))
        //    {
        //        query.CombineCritia(u => u.CarTypeName.Contains(request.CarTypeNameKey));
        //    }
        //    var pageSpec = query.New().ApplyOrderByDescending(a => a.CreateTime).ApplyPaging(new Pagination(request.page, request.limit));

        //    var (count, data) = await this.LoadPageAsNoTrackingAsync(query, pageSpec);
        //    return new TableData { count = count, data = data };

        //}

        //public async Task<StockTaskHistory> GetLastEntity(GetStockTaskHistoryEntityInput input)
        //{
        //    var query = new Specification<StockTaskHistory>(u => !u.IsDeleted);
        //    var currentWarehouseId = _appConfiguration.Value.WarehouseId;
        //    query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
        //    if (input.AreaId != null)
        //    {
        //        query.CombineCritia(u => u.AreaId == input.AreaId);
        //    }

        //    if (input.EquipmentId != null)
        //    {
        //        query.CombineCritia(u => u.EquipmentId == input.EquipmentId);
        //    }
        //    if (input.Status != null)
        //    {
        //        query.CombineCritia(u => (int)u.TaskStatus == input.Status);
        //    }
        //    return await this.Repository.Query(query.ApplyOrderByDescending(u => u.CompleteTime)).AsNoTracking().FirstOrDefaultAsync();
        //}

        //public async Task<GetTongJiTaskHistroyOutput> GetTongJiTaskHistroy()
        //{
        //    GetTongJiTaskHistroyOutput result = new GetTongJiTaskHistroyOutput();

        //    var query = new Specification<StockTaskHistory>(u => !u.IsDeleted);

        //    var currentWarehouseId = _appConfiguration.Value.WarehouseId;
        //    query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
        //    string beginTime = DateTime.Now.Year + "-01-01";
        //    query.CombineCritia(u => u.CreateTime >= DateTime.Parse(beginTime));

        //    var list = await this.Repository.Query(query).AsNoTracking().ToListAsync();


        //    result.TodayInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString())).Count();

        //    result.TodayOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString())).Count();
        //    var year = DateTime.Now.Year;
        //    var month = DateTime.Now.Month;
        //    List<int> YearInStockCountList = new List<int>();
        //    List<int> YearOutStockCountList = new List<int>();
        //    for (var i = 1; i <= 12; i++)
        //    {
        //        if (month >= i)
        //        {
        //            var beginDate = year + "-" + i + "-01";
        //            var endDate = "";
        //            if (i == 12)
        //            {
        //                endDate = (year + 1) + "-01-01";
        //            }
        //            else
        //            {
        //                endDate = year + "-" + (i + 1) + "-01";

        //            }
        //            var YearInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= DateTime.Parse(beginDate) && a.CreateTime < DateTime.Parse(endDate)).Count();
        //            var YearOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= DateTime.Parse(beginDate) && a.CreateTime < DateTime.Parse(endDate)).Count();
        //            YearInStockCountList.Add(YearInStockCount);
        //            YearOutStockCountList.Add(YearOutStockCount);
        //        }
        //        else
        //        {
        //            YearInStockCountList.Add(0);
        //            YearOutStockCountList.Add(0);
        //        }
        //    }

        //    result.YearInStockCount = YearInStockCountList.ToArray();
        //    result.YearOutStockCount = YearOutStockCountList.ToArray();
        //    return result;
        //}

        ///// <summary>
        ///// 前端饼图中的统计数据以及统计堆垛机出入库数量
        ///// </summary>
        ///// <returns></returns>
        //public async Task<GetTongJiDayCount> GetTongJiTaskDayHistroy()
        //{
        //    GetTongJiDayCount result = new GetTongJiDayCount();

        //    var query = new Specification<StockTaskHistory>(u => !u.IsDeleted);

        //    var currentWarehouseId = _appConfiguration.Value.WarehouseId;
        //    query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
        //    string beginTime = DateTime.Now.Year + "-01-01";
        //    query.CombineCritia(u => u.CreateTime >= DateTime.Parse(beginTime));

        //    var list = await this.Repository.Query(query).AsNoTracking().ToListAsync();


        //    result.TodayInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString())).Count();//今日入库数量
        //    result.TodayOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString())).Count();//今日出库数量

        //    result.E1TodayInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString()) && a.EquipmentId == 1).Count();//今日一号机入库数量
        //    result.E1TodayOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString()) && a.EquipmentId == 1).Count();//今日一号机出库数量
        //    if (currentWarehouseId == 1)
        //    {
        //        result.E2TodayInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString()) && a.EquipmentId == 2).Count();//今日二号机入库数量
        //        result.E2TodayOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString()) && a.EquipmentId == 2).Count();//今日二号机出库数量
        //        result.E3TodayInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString()) && a.EquipmentId == 3).Count();//今日三号机入库数量
        //        result.E3TodayOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString()) && a.EquipmentId == 3).Count();//今日三号机出库数量
        //    }
        //    else
        //    {
        //        //其他夹具库未有二号机和三号机
        //        result.E2TodayInStockCount = 0;//今日二号机入库数量
        //        result.E2TodayOutStockCount = 0;//今日二号机出库数量
        //        result.E3TodayInStockCount = 0;//今日三号机入库数量
        //        result.E3TodayOutStockCount = 0;//今日三号机出库数量
        //    }


        //    //获取当前时间
        //    DateTime today = DateTime.Now.Date;
        //    //获取本周一的日期（如果今天是周一返回当天）
        //    int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        //    DateTime StartofWeek = today.AddDays(-1 * diff).Date;//本周一
        //    DateTime EndofWeek = StartofWeek.AddDays(7);//下周一(不包括)
        //    result.E1WeekInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= StartofWeek && a.CreateTime < EndofWeek && a.EquipmentId == 1).Count();//一号机本周入库数量
        //    result.E1WeekOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= StartofWeek && a.CreateTime < EndofWeek && a.EquipmentId == 1).Count();//一号机本周出库数量

        //    if (currentWarehouseId == 1)
        //    {
        //        result.E2WeekInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= StartofWeek && a.CreateTime < EndofWeek && a.EquipmentId == 2).Count();//二号机本周入库数量
        //        result.E2WeekOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= StartofWeek && a.CreateTime < EndofWeek && a.EquipmentId == 2).Count();//二号机本周出库数量
        //        result.E3WeekInStockCount = list.Where(a => a.TaskType == TaskType.入库 && a.CreateTime >= StartofWeek && a.CreateTime < EndofWeek && a.EquipmentId == 3).Count();//三号机本周入库数量
        //        result.E3WeekOutStockCount = list.Where(a => a.TaskType == TaskType.出库 && a.CreateTime >= StartofWeek && a.CreateTime < EndofWeek && a.EquipmentId == 3).Count();//三号机本周出库数量
        //    }
        //    else
        //    {
        //        result.E2WeekInStockCount = 0;//二号机本周入库数量
        //        result.E2WeekOutStockCount = 0;//二号机本周出库数量
        //        result.E3WeekInStockCount = 0;//三号机本周入库数量
        //        result.E3WeekOutStockCount = 0;//三号机本周出库数量

        //    }



        //    // 获取本月第一天
        //    DateTime StartOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // 获取下个月第一天（即本月最后一天+1天）
        //    DateTime EndOfMonth = StartOfMonth.AddMonths(1);
        //    result.E1MonthInStockCount = list.Where(a => a.TaskType == Entities.TaskType.入库 && a.CreateTime >= StartOfMonth && a.CreateTime < EndOfMonth && a.EquipmentId == 1).Count();//一号机本月入库数量
        //    result.E1MonthOutStockCount = list.Where(a => a.TaskType == Entities.TaskType.出库 && a.CreateTime >= StartOfMonth && a.CreateTime < EndOfMonth && a.EquipmentId == 1).Count();//一号机本月出库数量
        //    if (currentWarehouseId==1)
        //    {
        //        result.E2MonthInStockCount = list.Where(a => a.TaskType == Entities.TaskType.入库 && a.CreateTime >= StartOfMonth && a.CreateTime < EndOfMonth && a.EquipmentId == 2).Count();//二号机本月入库数量
        //        result.E2MonthOutStockCount = list.Where(a => a.TaskType == Entities.TaskType.出库 && a.CreateTime >= StartOfMonth && a.CreateTime < EndOfMonth && a.EquipmentId == 2).Count();//二号机本月出库数量
        //        result.E3MonthInStockCount = list.Where(a => a.TaskType == Entities.TaskType.入库 && a.CreateTime >= StartOfMonth && a.CreateTime < EndOfMonth && a.EquipmentId == 3).Count();//三号机本月入库数量
        //        result.E3MonthOutStockCount = list.Where(a => a.TaskType == Entities.TaskType.出库 && a.CreateTime >= StartOfMonth && a.CreateTime < EndOfMonth && a.EquipmentId == 3).Count();//三号机本月出库数量

        //    }
        //    else
        //    {
        //        result.E2MonthInStockCount = 0;//二号机本月入库数量
        //        result.E2MonthOutStockCount = 0;//二号机本月出库数量
        //        result.E3MonthInStockCount = 0;//三号机本月入库数量
        //        result.E3MonthOutStockCount = 0;//三号机本月出库数量

        //    }

        //    //查询入库车型所有的数据
        //    var RuList = list.Where(u=>u.TaskType==Entities.TaskType.入库 && u.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString())).GroupBy(u=>u.CarTypeName).Select(g=>new HistoryCartype { CarName=g.Key,CountNumber=g.Count()}).ToList();
        //    if (RuList.Count!=0)
        //    {
        //        foreach (var item in RuList)
        //        {
        //            result.HistoryCartypesIn.Add(new HistoryCartype { CarName = item.CarName,CountNumber=item.CountNumber});
        //        }

        //    }
        //    //查询出库车型所有的数据
        //    var ChuList = list.Where(u => u.TaskType == Entities.TaskType.出库 && u.CreateTime >= DateTime.Parse(DateTime.Now.ToShortDateString())).GroupBy(u => u.CarTypeName).Select(g => new HistoryCartype { CarName = g.Key, CountNumber = g.Count() }).ToList();
        //    if (ChuList.Count != 0)
        //    {
        //        foreach (var item in ChuList)
        //        {
        //            result.HistoryCartypesOut.Add(new HistoryCartype { CarName = item.CarName, CountNumber = item.CountNumber });
        //        }

        //    }


        //    return result;
        //}

        #endregion


    }
}
