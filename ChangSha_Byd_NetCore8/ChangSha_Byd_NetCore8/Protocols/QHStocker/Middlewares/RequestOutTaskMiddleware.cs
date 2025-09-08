using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    /// <summary>
    /// 请求发送出库任务
    /// </summary>
    public class RequestOutTaskMiddleware : IWorkMiddleware<ScanContext>
    {
        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                //Console.WriteLine("RequestOutTaskMiddleware");
            }
            finally
            {
                await next(context);
            }
        }
    }
}
