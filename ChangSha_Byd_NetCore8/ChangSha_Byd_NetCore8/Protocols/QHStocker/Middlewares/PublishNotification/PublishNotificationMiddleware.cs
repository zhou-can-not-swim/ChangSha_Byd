using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model;
using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares.PublishNotification
{
    public class PublishNotificationMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly IMediator _mediator;


        /// <summary>
        /// 向web端发送初始化数据
        /// </summary>
        /// <param name="mediator"></param>
        public PublishNotificationMiddleware(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                //mst和plc信息同步
                var beated = GeneralHelper.CheckPlcHeartBeatSynced(context.PlcInfo, context.MstInfo);
                //将这里的同步消息发送给客户端
                await this._mediator.Publish(new ScanContextNotification(context, beated));

            }
            finally
            {
                await next(context);
            }
        }
    }
}
