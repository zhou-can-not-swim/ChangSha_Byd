using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    /// <summary>
    /// 完成任务
    /// </summary>
    public class FinishedTaskMiddleware : IWorkMiddleware<ScanContext>
    {

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                //Console.WriteLine("FinishedTaskMiddleware");
            }
            finally
            {
                await next(context);
            }
        }
    }
}
