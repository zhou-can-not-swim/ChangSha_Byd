using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Attr;
using System.Runtime.InteropServices;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc
{
    /// <summary>
    /// plc的库口的信号信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PlcMsg_Gateway
    {
        public PlcMsg_GatewayFlags Plc信号灯;
        public byte 检验结果;//0是初始化，1是校验通过，2是校验不通过
                         //public RFID_UID 入库来料RFID;
        [Endian(Endianness.BigEndian)]
        public short 入库来料RFID;
    }



    [Serializable]
    [Flags]
    public enum PlcMsg_GatewayFlags : byte
    {
        就位请求 = 1 << 0,
        允许出库 = 1 << 1
    }
}
