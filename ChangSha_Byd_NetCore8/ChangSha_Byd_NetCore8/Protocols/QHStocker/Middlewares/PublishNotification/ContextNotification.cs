using ChangSha_Byd_NetCore8.Extends.Scan;
using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares.PublishNotification
{
    /// <summary>
    /// 与plc连接时的准备，在publishNotificationMiddleware中设置好了值，可以在这里面处理后交给handler处理
    /// </summary>
    public class ScanContextNotification : INotification
    {
        public ScanContextNotification(ScanContext ctx, bool plcBeatSynced)
        {
            this.Context = ctx ?? throw new System.ArgumentNullException(nameof(ctx));
            this.PlcHeartBeated = plcBeatSynced;
            if (plcBeatSynced)
            {
                this.PlcHeartBeatedAt = ctx.CreatedAt;
            }
        }

        public ScanContext Context { get; }

        /// <summary>
        /// PLC心跳是否同步？
        /// </summary>
        public bool PlcHeartBeated { get; }
        /// <summary>
        /// PLC心跳同步时间？
        /// </summary>
        public DateTime PlcHeartBeatedAt { get; }
    }
}
