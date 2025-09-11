using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Log;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc;
using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    /// <summary>
    /// 入口就位，添加入库任务
    /// </summary>
    public class EntryArrivedMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly IMediator _mediator;

        public EntryArrivedMiddleware(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                context.MstMsg.GateWay.EC010_A库口 = await this.EntryArrivedBase(context.PlcInfo.EC010_A库口, context.MstMsg.GateWay.EC010_A库口, QH_EntryLocation.EC010_A工位入口);
                context.MstMsg.GateWay.EC010_B库口 = await this.EntryArrivedBase(context.PlcInfo.EC010_B库口, context.MstMsg.GateWay.EC010_A库口, QH_EntryLocation.EC010_B工位入口);
            }
            catch (Exception e)
            {
                Console.WriteLine("异常："+e);
            }
            finally
            {
                await next(context);
            }
        }

        //mst的一个库口信息，包括(100.0)就位确认和rfid等信息
        public async Task<MstMsg_GateWay> EntryArrivedBase(
            PlcInfo_Gateway plcInfo_Gateway,
            MstMsg_GateWay mstMsg_GateWay,//最原始的mst 字节信息
            QH_EntryLocation Location)
        {
            if (plcInfo_Gateway.StandByReq &&
                !string.IsNullOrEmpty(plcInfo_Gateway.EntryRFID) &&
                (!plcInfo_Gateway.EntryRFID.Equals("0")) &&
                !mstMsg_GateWay.Mst信号灯.HasFlag(MstMsg_GatewayFlags.就位确认))//mst的就位确认为false
            {

                var RFID16 = int.Parse(plcInfo_Gateway.EntryRFID).ToString("X");//将10进制RFID转换为16进制

                #region 给前端的 请求入库 日志
                var logMessage = new LogMessage()//new一下
                {
                    Content = Location + "请求入库，请求的(十进制)RFID:" + plcInfo_Gateway.EntryRFID + ":16进制RFID为" + RFID16,
                    Level = LogLevel.Information,
                    Timestamp = DateTime.Now,
                };
                var logmsg = new UILogNotificatinon(logMessage);
                await this._mediator.Publish(logmsg);//通知（notification）广播出去，让系统中所有“订阅了”这类通知的处理器（handler）依次执行它们的逻辑
                                                     //在UILogNotificationHandler类中进行处理
                #endregion

                var response = await _mediator.Send(new InStockArrivedRequest
                    { InStockNo = (int)Location, Rfid16 = RFID16, RFIDTen = plcInfo_Gateway.EntryRFID }
                );

                if (response.IsSucess)
                {
                    //对 11 块 100.0的操作
                    MstFlagsGatewayBuilder builder = new MstFlagsGatewayBuilder(mstMsg_GateWay.Mst信号灯);
                    mstMsg_GateWay.Mst信号灯 = builder.入口就位确认(true).Build();
                }
                else
                {
                    throw new System.Exception($"{Location}请求入库失败，原因：{response.Msg}");
                }

                return mstMsg_GateWay;
            }

            //plc 100.0 入库就位请求 false
            //11 块 100.0 入库就位确认 true    在前面入库成功的时候已经是true了
            if (!plcInfo_Gateway.StandByReq && mstMsg_GateWay.Mst信号灯.HasFlag(MstMsg_GatewayFlags.就位确认))
            {
                MstFlagsGatewayBuilder builder = new MstFlagsGatewayBuilder(mstMsg_GateWay.Mst信号灯);
                mstMsg_GateWay.Mst信号灯 = builder.入口就位确认(false).Build();
            }
            return mstMsg_GateWay;

        }
    }
}
