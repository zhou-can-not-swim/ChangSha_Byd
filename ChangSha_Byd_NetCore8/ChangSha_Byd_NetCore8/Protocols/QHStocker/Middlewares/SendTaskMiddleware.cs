using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request;
using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    /// <summary>
    /// 发送任务扫描stockTask表
    /// </summary>
    public class SendTaskMiddleware : IWorkMiddleware<ScanContext>
    {

        private readonly ILogger<SendTaskMiddleware> _logger;
        private readonly IMediator _mediator;

        public SendTaskMiddleware(ILogger<SendTaskMiddleware> logger, IMediator mediator)
        {
            this._logger = logger;
            this._mediator = mediator;
        }
        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {
                //如果发送任务之后PLC校验通过，需要将下发任务号和校验码清值
                //上位机发送 下发任务请求
                if (context.PlcInfo.VerificationCode == 1 &&// 13块 20
                    context.MstMsg.Core.VerificationCode != 0 && //上位机的校验号
                    context.MstMsg.Core.TaskNo != 0)        //上位机下发的任务号
                {
                    //提前清除任务号和校验码
                    context.Pending.Core.TaskNo = 0;
                    context.Pending.Core.VerificationCode = 0;
                }

                //plc设备的三种状态
                if (context.PlcInfo.StorkerStatus == StockerStatus.联机 &&
                    context.PlcInfo.StockerTrip == StockerTrip.待机 &&
                    context.PlcInfo.StockerCargo == StockerCargo.无货 &&
                    !context.PlcInfo.SendTaskAck &&
                    !context.MstInfo.SendTaskReq &&
                    !context.PlcInfo.FinishTaskReq &&
                    !context.MstInfo.FinishTaskAck)
                {
                    //后地板 + 纵梁规则: 纵梁优先，后地板再后 ，先入再出再维修

                    SendTaskResponse response = await this.GetTask(context);
                    if (response != null)//如果当前有任务，则下发任务给plc
                    {
                        #region  发送任务给plc

                        //将下发任务请求设置为1，三个状态都ok的话，上位机自动下发任务请求
                        MstFlagsGeneralBuilder builder = new MstFlagsGeneralBuilder(context.Pending.GeneralCmdWord);
                        context.Pending.GeneralCmdWord = builder.下发任务请求(true).Build();
                        //往上位机中添加在前面获取的任务
                        context.Pending.Core.TaskNo = (ushort)response.TaskCode;
                        context.Pending.Core.TaskType = response.TaskType;
                        context.Pending.Core.StartLine = (ushort)response.StartLine;
                        context.Pending.Core.StartFloor = (ushort)response.StartFloor;
                        context.Pending.Core.StartColumn = (ushort)response.StartColumn;
                        context.Pending.Core.EndLine = (ushort)response.EndLine;
                        context.Pending.Core.EndFloor = (ushort)response.EndFloor;
                        context.Pending.Core.EndColumn = (ushort)response.EndColumn;

                        context.Pending.Core.VerificationCode = response.VerificationCode;
                        context.Pending.Core.TaskRFID = short.Parse(response.TaskRFID);
                        if (response.TaskType == PLCTaskType.出库作业)
                        {
                            //写出库口rfid   在类QHRequestOutTaskMessageHander中进行处理
                            var ResponseRFID = await _mediator.Send(new RequestTaskRequest { RFid = response.TaskRFID });
                            //(工位出库口 rfid context)
                            this.WriteRFID(ResponseRFID.outLocation, response.TaskRFID, context);
                        }
                        #endregion
                    }
                }

                //上位机下发任务请求 下位机发送任务请求确认 
                if (context.MstMsg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.下发任务请求) &&
                    context.PlcInfo.SendTaskAck
                )
                {
                    MstFlagsGeneralBuilder builder = new MstFlagsGeneralBuilder(context.Pending.GeneralCmdWord);
                    context.Pending.GeneralCmdWord = builder.下发任务请求(false).Build();//立刻将下位机发送任务请求确认 的灯取消
                }

                //plc给完成任务请求的时候，上位机同时也要确认  --> 清除RFID
                if (context.PlcInfo.FinishTaskReq && context.MstInfo.FinishTaskAck)
                {
                    #region  清除上位机下发信号
                    context.Pending.Core.TaskNo = 0;
                    context.Pending.Core.TaskType = 0;
                    context.Pending.Core.StartLine = 0;
                    context.Pending.Core.StartFloor = 0;
                    context.Pending.Core.StartColumn = 0;
                    context.Pending.Core.EndLine = 0;
                    context.Pending.Core.EndFloor = 0;
                    context.Pending.Core.EndColumn = 0;
                    context.Pending.Core.VerificationCode = 0;
                    context.Pending.Core.TaskRFID = 0;
                    #endregion

                    context.Pending.GateWay.EC010_A库口.请求出库RFID = 0;
                    context.Pending.GateWay.EC010_B库口.请求出库RFID = 0;
                }



            }
            catch (Exception e)
            {
                Console.WriteLine("SendTaskMiddleware异常："+e);
            }
            finally
            {
                await next(context);
            }


        }

        public ScanContext WriteRFID(QH_OutLocation outLocation, string TaskRFID, ScanContext context)
        {
            //找到对应的工位出口，写入对应的RFID，也就是CarTypeNum,和rfid是一个值
            if (outLocation == QH_OutLocation.EC010_A工位出口) context.Pending.GateWay.EC010_A库口.请求出库RFID = short.Parse(TaskRFID);
            if (outLocation == QH_OutLocation.EC010_B工位出口) context.Pending.GateWay.EC010_B库口.请求出库RFID = short.Parse(TaskRFID);//请求出库RFID 页面上的

            return context;
        }

        public async Task<SendTaskResponse> GetTask(ScanContext context)
        {
            //任务优先级： 移库任务，入库任务，出库任务，维修出库任务          优先级应该是在handler获取任务中if else进行的顺序
            //区域优先级：机舱 EC 	, 前地板 FF ,后地板 RF , 后纵梁 RSM

            //在QHSendTaskMessageHander文件中处理request
            SendTaskResponse response = null;
            #region 移库任务
            //新建一个request，和response配合使用？？？
            SendTaskRequest request = new SendTaskRequest()
            {
                EquipmentId = 1,
                TaskStatus = Common.TaskStatus.等待执行,
                TaskType = TaskType.移库,
                IsRepair = false
            };
            response = await _mediator.Send(request);
            if (response != null)
            {
                return response;
            }
            #endregion

            #region  EC的出库
            SendTaskRequest request1 = new SendTaskRequest()
            {
                EquipmentId = 1,
                TaskStatus = Common.TaskStatus.已校验等待出库,
                TaskType = TaskType.出库,
                AreaId = 1,
                IsRepair = false,
                OutGateWayStatusList = this.GetECOutStatus(context)//获取EC区域的出库状态

            };
            //在文件 QHSendTaskMessageHander 中进行处理
            response = await _mediator.Send(request1);
            if (response != null)
            {
                return response;
            }
            #endregion

            #region EC入库
            SendTaskRequest request7 = new SendTaskRequest()
            {
                EquipmentId = 1,
                TaskStatus = Common.TaskStatus.等待执行,
                TaskType = TaskType.入库,
                AreaId = 1,
                IsRepair = false
            };
            response = await _mediator.Send(request7);
            if (response != null)
            {
                return response;
            }
            #endregion

            return response;


        }

        /// <summary>
        /// EC的出口状态(机舱)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<OutGateWayStatusItem> GetECOutStatus(ScanContext context)
        {
            List<OutGateWayStatusItem> OutStatusList = new List<OutGateWayStatusItem>();
                                                          // AB出口状态 和库口对应状态（由plc进行设置的）
            OutStatusList.Add(new OutGateWayStatusItem() { outLocation = QH_OutLocation.EC010_A工位出口, OutStatus = context.PlcInfo.EC010_A库口.OutStatus });
            OutStatusList.Add(new OutGateWayStatusItem() { outLocation = QH_OutLocation.EC010_B工位出口, OutStatus = context.PlcInfo.EC010_B库口.OutStatus });

            return OutStatusList;

        }
    }
}