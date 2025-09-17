using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.SnapShot
{
    public static class ScanContextExtensions
    {
        public static ScanSnapshot ToSnapshot(this ScanContext ctx)
        {
            var plc = ctx.PlcInfo;
            var mst = ctx.MstInfo;

            return new ScanSnapshot
            {
                CreatedAt = ctx.CreatedAt,
                PlcInfo = new PlcSnapshot
                {
                    HeartBeatAck = plc.heartBeatAck,
                    HeartBeatReq = plc.heartBeatReq,
                    SendTaskAck = plc.SendTaskAck,
                    CurrentFloor = plc.CurrentFloor,
                    CurrentLine = plc.CurrentLine,
                    CurrentColumn = plc.CurrentColumn,
                    DoTaskNo = plc.DoTaskNo,
                    TaskNo = plc.TaskNo,

                    StorkerStatus = plc.StorkerStatus,
                    StockerTrip = plc.StockerTrip,
                    StockerCargo = plc.StockerCargo,

                    Gateways = new Dictionary<string, PlcGatewayDto>
                    {
                        ["EC010_A库口"] = plc.EC010_A库口.ToDto(),
                        ["EC010_B库口"] = plc.EC010_B库口.ToDto(),
                        // 后续继续加
                    }
                },
                MstInfo = new MstSnapshot
                {
                    HeartBeatReq = mst.heartBeatReq,
                    SendTaskReq = mst.SendTaskReq,
                    TaskNo = mst.TaskNo,
                    StartLine = mst.StartLine,
                    StartFloor = mst.StartFloor,
                    StartColumn = mst.StartColumn,
                    EndLine = mst.EndLine,
                    EndFloor = mst.EndFloor,
                    EndColumn = mst.EndColumn,
                    TaskRFID = mst.TaskRFID,
                    Gateways = new Dictionary<string, MstGatewayDto>
                    {
                        ["EC010_A库口"] = mst.EC010_A库口.ToDto(),
                        ["EC010_B库口"] = mst.EC010_B库口.ToDto(),
                    }
                }
            };
        }
    }
}
