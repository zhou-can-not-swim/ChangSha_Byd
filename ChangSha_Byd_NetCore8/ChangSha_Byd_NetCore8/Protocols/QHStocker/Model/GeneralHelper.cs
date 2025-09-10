using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model
{
    public static class GeneralHelper
    {
        public static bool CheckPlcHeartBeatSynced(PlcInfo plc, MstInfo mst)
        {
            if (plc is null)
            {
                throw new ArgumentNullException(nameof(plc));
            }

            if (mst is null)
            {
                throw new ArgumentNullException(nameof(mst));
            }
            // mst当前心跳请求 检查当前枚举值是否包含指定的标志
            var mstreq = mst.heartBeatReq;
            // plc当前心跳响应
            var plcack = plc.heartBeatAck;

            //比较mst的心跳请求和plc的心跳响应是否一致
            return mstreq == plcack;
        }
    }
}
