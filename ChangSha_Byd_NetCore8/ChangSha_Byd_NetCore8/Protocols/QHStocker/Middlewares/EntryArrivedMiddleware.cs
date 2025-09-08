using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    /// <summary>
    /// 入口就位，添加入库任务
    /// </summary>
    public class EntryArrivedMiddleware : IWorkMiddleware<ScanContext>
    {


        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                context.MstMsg.GateWay.EC010_A库口 = await this.EntryArrivedBase(context.PlcInfo.EC010_A库口, context.MstMsg.GateWay.EC010_A库口);
                context.MstMsg.GateWay.EC010_B库口 = await this.EntryArrivedBase(context.PlcInfo.EC010_B库口, context.MstMsg.GateWay.EC010_B库口);
            }
            finally
            {
                await next(context);
            }
        }

        //mst的一个库口信息，包括就位确认和rfid等信息
        public async Task<MstMsg_GateWay> EntryArrivedBase(
            PlcInfo_Gateway plcInfo_Gateway,
            MstMsg_GateWay mstMsg_GateWay)
        {
            if (plcInfo_Gateway.StandByReq &&
                !string.IsNullOrEmpty(plcInfo_Gateway.EntryRFID) &&
                (!plcInfo_Gateway.EntryRFID.Equals("0")) &&
                !mstMsg_GateWay.Mst信号灯.HasFlag(MstMsg_GatewayFlags.就位确认))//mst的就位确认为false
            {
                await Task.Delay(1000); // 模拟处理时间
                Console.WriteLine("入库操作");

                // 入库成功之后
                if (true)
                {
                    //MstFlagsGatewayBuilder builder = new MstFlagsGatewayBuilder(mstMsg_GateWay.Mst信号灯);
                    //mstMsg_GateWay.Mst信号灯 = builder.入口就位确认(true).Build();
                }


                return mstMsg_GateWay;
            }
            return mstMsg_GateWay;
        }
    }
}
