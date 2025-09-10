using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares.PublishNotification;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker
{
    /// <summary>
    /// 处理器
    /// </summary>
    public class ScanProcessor
    {
        private WorkDelegate<ScanContext> BuildContainer()
        {
            var container = new WorkBuilder<ScanContext>()
                  .Use<PublishNotificationMiddleware>()//发布消息给Web
                  .Use<EntryArrivedMiddleware>()//就位请求
                  .Use<RequestOutTaskMiddleware>()//请求出库
                  .Use<SendTaskMiddleware>()//发送任务
                  .Use<FinishedTaskMiddleware>()//任务完成
                  .Use<HeartBeatMiddleware>()//心跳交互
                  .Use<ReadAndWritePlcMiddleware>()//前端进行plc读写,后续可以将下一个组件进行合并
                  .Use<FlushPendingMiddleware>()//写plc
            .Build();

            return container;
        }

        public async Task HandleAsync(ScanContext ctx)
        {
            var workcontainer = BuildContainer();
            await workcontainer.Invoke(ctx);
        }

    }
}
