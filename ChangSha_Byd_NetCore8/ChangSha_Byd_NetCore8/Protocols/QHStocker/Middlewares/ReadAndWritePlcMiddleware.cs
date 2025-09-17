using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model;
using Microsoft.Extensions.Caching.Memory;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    public class ReadAndWritePlcMiddleware : IWorkMiddleware<ScanContext>
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IMemoryCache _cache;

        //public ReadAndWritePlcMiddleware(
        //    IHttpContextAccessor httpContextAccessor,
        //    IMemoryCache cache)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //    //_requestDelegate = requestDelegate;
        //    _cache = cache;

        //}

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            //var plc = _cache.Get<plcLink>("plc");

            //if (plc != null)
            //{
            //    MstFlagsGeneralBuilder builder = new MstFlagsGeneralBuilder(context.Pending.GeneralCmdWord);

            //    context.Pending.GeneralCmdWord = builder.下发任务请求(plc.sendTaskReq).Build();
            //    context.Pending.GeneralCmdWord = builder.完成任务确认(plc.finishTaskAck).Build();
            //    //清除缓存
            //    _cache.Remove("plc");
            //}
            await next(context);

        }

    }

}
