using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Log;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request;
using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    /// <summary>
    /// 请求发送出库任务
    /// </summary>
    public class RequestOutTaskMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly IMediator _mediator;

        public RequestOutTaskMiddleware(IMediator mediator)
        {
            this._mediator = mediator;
        }
        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                bool IsRequestTaskRfidAllNull = this.IsRequestTaskRfidAllNull(context); //所有的库口请求出库RFID都是空的则返回true ，否则返回false

                //plc的三种状态修改，进入 25 41 61 还需要出库确认信号
                if (!context.MstInfo.RequsetOutTaskReq
                    && !context.PlcInfo.RequestOutTaskAck
                    && context.PlcInfo.StockerTrip == StockerTrip.待机
                    && context.PlcInfo.StorkerStatus == StockerStatus.联机
                    && context.PlcInfo.StockerCargo == StockerCargo.无货
                    && context.PlcInfo.VerificationCode == 0
                    && IsRequestTaskRfidAllNull)
                {
                    //在这个类中进行处理：QHRequestOutTaskMessageHander
                    //获得 出库口号 和 rfid
                    var response = await _mediator.Send(new RequestTaskRequest { });
                    if (response != null && response.TaskRFID != null)
                    {
                        #region log
                        var logMessage = new LogMessage()
                        {
                            Content = "向PLC发送请求出库，请求库口是" + response.outLocation + ",RFID是" + response.TaskRFID,
                            Level = LogLevel.Information,
                            Timestamp = DateTime.Now,
                        };
                        var logmsg = new UILogNotificatinon(logMessage);
                        await this._mediator.Publish(logmsg);
                        #endregion

                        #region  发送RFID给plc，下发请求出库置为1
                        //在 上位机11 下发任务请求 设置为1
                        MstFlagsGeneralBuilder builder = new MstFlagsGeneralBuilder(context.Pending.GeneralCmdWord);
                        context.Pending.GeneralCmdWord = builder.下发请求出库(true).Build();
                        context = this.WriteRFID(response.outLocation, response.TaskRFID, context);//写请求的出库口RFID
                        #endregion
                    }
                }
            }
            finally
            {
                await next(context);
            }
        }

        public bool IsRequestTaskRfidAllNull(ScanContext context)
        {
            if (!string.IsNullOrEmpty(context.MstInfo.EC010_A库口.RequestTaskRFID)) return false;
            if (!string.IsNullOrEmpty(context.MstInfo.EC010_B库口.RequestTaskRFID)) return false;
            return true;
        }

        /// <summary>
        /// 写库口的请求出库RFID
        /// </summary>
        /// <param name="outLocation"></param>
        /// <param name="TaskRFID"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public ScanContext WriteRFID(QH_OutLocation outLocation, string TaskRFID, ScanContext context)
        {
            if (outLocation == QH_OutLocation.EC010_A工位出口) context.Pending.GateWay.EC010_A库口.请求出库RFID = short.Parse(TaskRFID);
            if (outLocation == QH_OutLocation.EC010_B工位出口) context.Pending.GateWay.EC010_B库口.请求出库RFID = short.Parse(TaskRFID);

            return context;
        }
    }
}