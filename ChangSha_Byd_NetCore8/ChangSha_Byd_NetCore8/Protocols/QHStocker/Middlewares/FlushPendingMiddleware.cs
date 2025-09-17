using ChangSha_Byd_NetCore8.Extends;
using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using Microsoft.Extensions.Options;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    public class FlushPendingMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<FlushPendingMiddleware> _logger;
        private readonly IOptionsMonitor<ScanOpts> scanOptsMonitor;
        private readonly PlcMgr _mgr;

        public FlushPendingMiddleware(ILogger<FlushPendingMiddleware> logger, IOptionsMonitor<ScanOpts> scanOptsMonitor, PlcMgr mgr)
        {
            this._logger = logger;
            this.scanOptsMonitor = scanOptsMonitor;
            this._mgr = mgr;
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {  
            try
            {
                var scanOpts = this.scanOptsMonitor.CurrentValue;
                //向名为QHStocker的PLC设备写入数据（context.Pending)
                var res = await this._mgr.PlcName_QHStocker.SendCmdAsync(context.Pending);
                if (res.IsError)
                {
                    throw new System.Exception($"向PLC写数据错误：{res.ErrorValue}");
                }
            }
            finally
            {
                await next(context);
            }
        }
    }
}
