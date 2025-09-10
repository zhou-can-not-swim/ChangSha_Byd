using Byd.Services.Request;
using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.Hub;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ChangSha_Byd_NetCore8.App.Production
{

    //public class ProductionLogApp : FutureBaseEntityService<int, ProductionLog>
    //{
    //    private readonly IOptions<AppSetting> _appConfiguration;
    //    private readonly IHubContext<ProductionHub, IProductionHub> _hubContext;
    //    private readonly ILogger<ProductionLogApp> _logger;
    //    private readonly IMemoryCache _memoryCache;
    //    public ProductionLogApp(IGenericRepository<int, ProductionLog> repo, IOptions<AppSetting> appConfiguration, IHubContext<ProductionHub, IProductionHub> hubContext, ILogger<ProductionLogApp> logger, IMemoryCache memoryCache) : base(repo)
    //    {
    //        _appConfiguration = appConfiguration;
    //        _hubContext = hubContext;
    //        _logger = logger;
    //        _memoryCache = memoryCache;
    //    }
    //    /// <returns></returns>
    //    public async Task<TableData> Load(QueryProductionLogReq request)
    //    {

    //        var query = new Specification<ProductionLog>(a => !a.IsDeleted);

    //        var currentWarehouseId = _appConfiguration.Value.WarehouseId;
    //        query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
    //        if (!string.IsNullOrEmpty(request.key))
    //        {
    //            query.CombineCritia(u => u.Msg.Contains(request.key));
    //        }


    //        if (request.LogType != null)
    //        {
    //            query.CombineCritia(u => u.LogType == request.LogType);
    //        }

    //        if (!string.IsNullOrEmpty(request.BeginTime))
    //        {
    //            query.CombineCritia(u => u.CreateTime >= DateTime.Parse(request.BeginTime));
    //        }
    //        if (!string.IsNullOrEmpty(request.EndTime))
    //        {
    //            query.CombineCritia(u => u.CreateTime < DateTime.Parse(request.EndTime).AddDays(1));
    //        }
    //        var pageSpec = query.New().ApplyOrderByDescending(a => a.CreateTime).ApplyPaging(new Pagination(request.page, request.limit));

    //        var (count, data) = await this.LoadPageAsNoTrackingAsync(query, pageSpec);
    //        return new TableData { count = count, data = data };
    //    }

    //    /// <summary>
    //    /// 新增日志
    //    /// </summary>
    //    /// <param name="EventSource"></param>
    //    /// <param name="Msg"></param>
    //    /// <param name="LogType">/ 日志类型 1正常；2错误信息</param>
    //    /// <returns></returns>
    //    public async Task<ProductionLog> Add(string EventSource, string Msg, int LogType)
    //    {
    //        ProductionLog entity = new ProductionLog()
    //        {
    //            WarehouseId = _appConfiguration.Value.WarehouseId,
    //            EventSource = EventSource,
    //            Msg = Msg,
    //            LogType = LogType,
    //            CreateTime = DateTime.Now
    //        };
    //        var a = await this.AddAsync(entity);
    //        return a.Data;
    //    }
    //    /// <summary>
    //    /// 展示报警错误信息
    //    /// </summary>
    //    /// <param name="alarmMessage"></param>
    //    /// <returns></returns>
    //    public async Task DealErrorMsg(AlarmMessage alarmMessage)
    //    {

    //        //推送到前端           
    //        await _hubContext.Clients.All.showAlarmMsg(alarmMessage);

    //        //记录到日志文件
    //        var msg = alarmMessage.EventSource + "报警,报警内容：" + alarmMessage.Content;
    //        this._logger.LogError(msg);
    //        string lblErrorMsg = _memoryCache.Get<string>(alarmMessage.EventSource);
    //        if (string.IsNullOrEmpty(lblErrorMsg) || (lblErrorMsg != alarmMessage.Content))
    //        {
    //            //记录到数据库,并且更新缓存
    //            if (alarmMessage.Level== LogLevel.Error)
    //            {
    //                //数据库只记录错的
    //               await this.Add(alarmMessage.EventSource, alarmMessage.Content, 2);
    //            }
            
    //            _memoryCache.Set(alarmMessage.EventSource, alarmMessage.Content);
    //        }

    //    }
      
    //}
}
