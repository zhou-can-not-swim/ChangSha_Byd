namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst
{
    /// <summary>
    /// 封装好的库口信息，对100.0就位确认信号灯和102请求出库rfid进行输入
    /// </summary>
    public class MstInfo_Gateway
    {
        private readonly MstMsg_GateWay _msg;

        public MstInfo_Gateway(ref MstMsg_GateWay msg)
        {
            this._msg = msg;
        }


        public bool StandByAck => this._msg.Mst信号灯.HasFlag(MstMsg_GatewayFlags.就位确认);

        public string RequestTaskRFID => this._msg.请求出库RFID.ToString();
    }
}
