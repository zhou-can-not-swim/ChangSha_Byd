using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    /// <summary>
    /// 发送任务扫描stockTask表
    /// </summary>
    public class SendTaskMiddleware : IWorkMiddleware<ScanContext>
    {

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                //Console.WriteLine("SendTaskMiddleware");
            }
            finally
            {
                await next(context);
            }
        }
    }
}
