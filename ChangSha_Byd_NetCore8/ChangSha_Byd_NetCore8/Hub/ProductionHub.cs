using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Log;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.SnapShot;
using Microsoft.AspNetCore.SignalR;

namespace ChangSha_Byd_NetCore8.Hub
{
    public interface IProductionHub
    {

        //Task receiveQHStockerMsg(ScanContext context);
        Task receiveQHStockerMsg(ScanSnapshot context);


        ///// <summary>
        ///// 显示报警信息
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //Task showAlarmMsg(AlarmMessage alarmmsg);

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        //Task showMsg(LogMessage alarmmsg);

    }


    public class ProductionHub : Hub<IProductionHub>
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<ProductionHub> _logger;

        public ProductionHub(
            IServiceProvider sp,
            ILogger<ProductionHub> logger
        )
        {
            this._sp = sp;
            this._logger = logger;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            this._logger.LogWarning($"客户端连接断开");
            return base.OnDisconnectedAsync(exception);
        }




    }
}
