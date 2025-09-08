using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    public class FlushPendingMiddleware : IWorkMiddleware<ScanContext>
    {

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                //Console.WriteLine("FlushPendingMiddleware");
            }
            finally
            {
                await next(context);
            }
        }
    }
}
