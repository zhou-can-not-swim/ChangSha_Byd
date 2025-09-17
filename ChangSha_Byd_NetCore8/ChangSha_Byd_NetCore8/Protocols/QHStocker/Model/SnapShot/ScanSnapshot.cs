using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.SnapShot
{
    // 给 SignalR 用的纯数据快照
    public sealed class ScanSnapshot
    {
        public DateTime CreatedAt { get; init; }
        public PlcSnapshot PlcInfo { get; init; }
        public MstSnapshot MstInfo { get; init; }
    }

    public sealed class PlcSnapshot
    {
        public bool HeartBeatAck { get; init; }
        public bool HeartBeatReq { get; init; }
        public bool SendTaskAck { get; init; }
        public int CurrentFloor { get; init; }
        public int CurrentLine { get; init; }
        public int CurrentColumn { get; init; }
        public int DoTaskNo { get; init; }
        public int TaskNo { get; init; }

        public StockerStatus StorkerStatus { get; init;}
        public StockerTrip StockerTrip { get; init;}
        public StockerCargo StockerCargo { get; init;}

        // 库口快照数组，KEY 用字符串
        public Dictionary<string, PlcGatewayDto> Gateways { get; init; }
    }

    public sealed class MstSnapshot
    {
        public bool HeartBeatReq { get; init; }
        public bool SendTaskReq { get; init; }
        public int TaskNo { get; init; }
        public int StartLine { get; init; }
        public int StartFloor { get; init; }
        public int StartColumn { get; init; }
        public int EndLine { get; init; }
        public int EndFloor { get; init; }
        public int EndColumn { get; init; }
        public short TaskRFID { get; init; }

        public Dictionary<string, MstGatewayDto> Gateways { get; init; }
    }


}
