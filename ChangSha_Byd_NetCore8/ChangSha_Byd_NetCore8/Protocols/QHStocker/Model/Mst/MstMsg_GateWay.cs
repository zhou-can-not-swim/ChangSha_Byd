using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Attr;
using System.Runtime.InteropServices;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst
{
    /// <summary>
    ///Mst一个库口的信号信息,就是每一个库口20B
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MstMsg_GateWay
    {
        public MstMsg_GatewayFlags Mst信号灯;//100
        public byte 预留;//101
                       //public RFID_UID 请求出库RFID; 大端序
        [Endian(Endianness.BigEndian)]
        public short 请求出库RFID;//102
    }


    [Serializable]
    [Flags]
    public enum MstMsg_GatewayFlags : byte
    {
        就位确认 = 1 << 0
    }
}
