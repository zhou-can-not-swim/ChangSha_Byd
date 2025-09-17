using ChangSha_Byd_NetCore8.Hub;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Log;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChangSha_Byd_NetCore8.Handler
{
    public class UILogNotificationHandler : INotificationHandler<UILogNotificatinon>
    {
        private readonly IHubContext<ProductionHub, IProductionHub> _hubContext;
        private readonly ILogger<UILogNotificationHandler> _logger;

        public UILogNotificationHandler(
            IHubContext<ProductionHub, IProductionHub> hubContext,
            ILogger<UILogNotificationHandler> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task Handle(UILogNotificatinon notification, CancellationToken cancellationToken)
        {

            //通过SignalR推送到前端
            //await _hubContext.Clients.All.showMsg(notification.LogMessage);
            this._logger.LogInformation(notification.LogMessage.Content);
        }

    }
}
