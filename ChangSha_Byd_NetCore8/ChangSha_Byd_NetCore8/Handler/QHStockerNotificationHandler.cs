using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.Hub;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares.PublishNotification;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.SnapShot;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace ChangSha_Byd_NetCore8.Handler
{
    /// <summary>
    /// ContextNotification的处理类
    /// </summary>
    public class QHStockerNotificationHandler : INotificationHandler<ScanContextNotification>
    {
        private readonly IHubContext<ProductionHub, IProductionHub> _hubContext;
        private IMemoryCache _objCache;

        public QHStockerNotificationHandler(
            IMemoryCache objCache,
            IHubContext<ProductionHub, IProductionHub> hubContext)
        {
            _objCache = objCache;
            _hubContext = hubContext;
        }

        public async Task Handle(ScanContextNotification notification, CancellationToken cancellationToken)
        {
            ScanContext context = notification.Context;
            //发送消息给客户端
            var snap = context.ToSnapshot();

            await _hubContext.Clients.All.receiveQHStockerMsg(snap);

        }
    }
}
