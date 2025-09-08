using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc;
using System.Text.Json.Serialization;

namespace ChangSha_Byd_NetCore8.Extends.Scan
{
    /// <summary>
    /// 扫描上下文，包括plc,mst信息，还有写入plc的pedding
    /// </summary>
    public class ScanContext : IWorkContext
    {
        public ScanContext(IServiceProvider sp, PlcMsg plcmsg, MstMsg mstmsg, DateTime createdAt)
        {
            this.ServiceProvider = sp;
            this.PlcMsg = plcmsg;
            this.PlcInfo = new PlcInfo(plcmsg);
            this.MstMsg = mstmsg;
            this.MstInfo = new MstInfo(mstmsg);
            this.Pending = new MstMsg(mstmsg);
            this.CreatedAt = createdAt;
        }

        /// <summary>
        /// 只读属性
        /// </summary>
        [JsonIgnore]
        public PlcMsg PlcMsg { get; }
        /// <summary>
        /// 只读属性
        /// </summary>
        public PlcInfo PlcInfo { get; set; }
        /// <summary>
        /// 只读属性
        /// </summary>
        public MstInfo MstInfo { get; }

        /// <summary>
        /// 只读属性（获取plcMst部分的值）
        /// </summary>
        public MstMsg MstMsg { get; }

        /// <summary>
        /// 只读属性（这个是上位机需要修改plcMst部分的值）
        /// 在FlushPendingMiddleware 里在Pending的值写给Plc
        /// </summary>
        [JsonIgnore]
        public MstMsg Pending { get; }

        [JsonIgnore]
        public IServiceProvider ServiceProvider { get; }

        public DateTime CreatedAt { get; }
    }
}
