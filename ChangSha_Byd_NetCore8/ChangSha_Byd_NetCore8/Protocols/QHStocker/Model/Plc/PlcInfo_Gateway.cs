namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc
{
    /// <summary>
    /// 对一个库口的100.0就位请求，100.1允许出库 101检验结果 102入库来料rfid进行的封装
    /// </summary>
    public class PlcInfo_Gateway
    {
        private readonly PlcMsg_Gateway _msg;

        public PlcInfo_Gateway(ref PlcMsg_Gateway msg)
        {
            this._msg = msg;
        }


        public bool StandByReq => this._msg.Plc信号灯.HasFlag(PlcMsg_GatewayFlags.就位请求);
        public bool OutStatus => this._msg.Plc信号灯.HasFlag(PlcMsg_GatewayFlags.允许出库);
        public int RequestTaskResult => this._msg.检验结果;
        public string EntryRFID => this._msg.入库来料RFID.ToString();
    }
}
