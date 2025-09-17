using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    public class HeartBeatMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<HeartBeatMiddleware> _logger;

        public HeartBeatMiddleware(ILogger<HeartBeatMiddleware> logger)
        {
            this._logger = logger;
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            MstFlagsGeneralBuilder builder = new MstFlagsGeneralBuilder(context.Pending.GeneralCmdWord);
            // 响应PLC心跳请求
            builder.响应PLC心跳请求(context.PlcInfo.heartBeatReq);

            // mst当前心跳请求
            var mstreq = context.Pending.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.心跳请求);
            // plc当前心跳响应
            var plcack = context.PlcInfo.heartBeatAck;

            if (mstreq == plcack)
            {
                this._logger.LogDebug($"PLC心跳响应和MST请求相匹配={mstreq}");
                // 切换MST心跳请求标志
                builder.下发心跳请求(!mstreq);
            }
            else
            {
                // ...
            }

            context.Pending.GeneralCmdWord = builder.Build();
            await next(context);
        }

    }
}
