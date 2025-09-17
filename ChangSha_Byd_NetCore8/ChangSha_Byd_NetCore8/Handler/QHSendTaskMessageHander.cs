using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App;
using ChangSha_Byd_NetCore8.App.Production;
using ChangSha_Byd_NetCore8.App.WarehouseModel;
using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.Hub;
using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Log;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChangSha_Byd_NetCore8.Handler
{
    /// <summary>
    /// 给堆垛机发送任务消息处理
    /// </summary>
    public class QHSendTaskMessageHander : IRequestHandler<SendTaskRequest, SendTaskResponse>
    {
        private readonly StockTaskApp _stockTaskApp;
        private readonly GatewayApp _gatewayApp;

        private readonly IHubContext<ProductionHub, IProductionHub> _hubContext;
        private readonly ILogger<QHSendTaskMessageHander> _logger;

        public QHSendTaskMessageHander(StockTaskApp stockTaskApp, GatewayApp gatewayApp, IHubContext<ProductionHub, IProductionHub> hubContext, ILogger<QHSendTaskMessageHander> logger)
        {
            _stockTaskApp = stockTaskApp;
            _gatewayApp = gatewayApp;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<SendTaskResponse> Handle(SendTaskRequest request, CancellationToken cancellationToken)
        {

            StockTask stockTask = null;
            int requestTaskType = (int)request.TaskType;
            //把request中的参数赋给getStockTaskEntityInput，我把它理解成Dto
            GetStockTaskEntityInput getStockTaskEntityInput = new GetStockTaskEntityInput()
            {
                EquipmentId = request.EquipmentId,//当前设备号（只有堆垛机）
                Status = (int)request.TaskStatus, //状态为1 等待执行 或者是6 已校验等待执行
                TaskType = (int?)request.TaskType,      // 可能是123 主要是为了下面去找出库还是入库还是移库任务
                AreaId = request.AreaId,
                IsRepair = request.IsRepair

            };


            #region  获取任务,只获取一条
            //移库任务
            if (requestTaskType == 3)
            {
                stockTask = await _stockTaskApp.GetStockTaskEntity(getStockTaskEntityInput);
            }
            else if (requestTaskType == 1 && !request.IsRepair)//获取入库任务
            {
                stockTask = await _stockTaskApp.GetStockTaskEntity(getStockTaskEntityInput);
            }
            else if (requestTaskType == 2 && !request.IsRepair)//获取出库任务
            {
                //得到所有的出库口 ，
                foreach (var item in request.OutGateWayStatusList)
                {
                    if (item.OutStatus == true)//true说明当前库口处于打开状态，如果为false说明有任务占用了
                    {
                        //给GatewayId
                        getStockTaskEntityInput.GatewayId = (int)item.outLocation;

                        //只拿打开库口的 任务
                        stockTask = await _stockTaskApp.GetStockTaskEntity(getStockTaskEntityInput);
                        if (stockTask != null)
                        {
                            break;
                        }
                    }
                }
            }
            else if (requestTaskType == 1 && request.IsRepair)//维修入库任务
            {
                stockTask = await _stockTaskApp.GetStockTaskEntity(getStockTaskEntityInput);
            }
            else if (requestTaskType == 2 && request.IsRepair)//维修出库任务
            {
                foreach (var item in request.OutGateWayStatusList)
                {
                    if (item.OutStatus == true)
                    {
                        getStockTaskEntityInput.GatewayId = (int)item.outLocation;
                        stockTask = await _stockTaskApp.GetStockTaskEntity(getStockTaskEntityInput);

                        if (stockTask != null)
                        {
                            break;
                        }
                    }

                }
            }

            #endregion

            //TODO 这里有设置排 列 层，这些是怎么生成的或者说是怎么来的
            //无任务 返回null
            //有任务 继续处理
            if (stockTask != null)
            {
                SendTaskResponse result = new SendTaskResponse();
                result.TaskCode = stockTask.TaskNo;
                Gateway gatewayEntity = null;
                if (stockTask.GatewayId != null)
                    gatewayEntity = await _gatewayApp.GetByIdAsNoTrackingAsync((int)stockTask.GatewayId);

                switch (stockTask.TaskType)
                {
                    case TaskType.入库:
                        result.TaskType = PLCTaskType.入库作业;

                        //取货层排列
                        var codesStart = gatewayEntity.LocationCode.Split('-');
                        result.StartLine = int.Parse(codesStart[0]);
                        result.StartColumn = int.Parse(codesStart[1]);
                        result.StartFloor = int.Parse(codesStart[2]);

                        //目的层排列
                        var codesEnd = stockTask.InLocationCode.Split('-');
                        result.EndLine = int.Parse(codesEnd[0]);
                        result.EndColumn = int.Parse(codesEnd[1]);
                        result.EndFloor = int.Parse(codesEnd[2]);

                        break;
                    case TaskType.出库:
                        result.TaskType = PLCTaskType.出库作业;

                        //取货层排列
                        var codesStart2 = stockTask.OutLocationCode.Split('-');
                        result.StartLine = int.Parse(codesStart2[0]);
                        result.StartColumn = int.Parse(codesStart2[1]);
                        result.StartFloor = int.Parse(codesStart2[2]);

                        //目的层排列
                        var codesEnd2 = gatewayEntity.LocationCode.Split('-');
                        result.EndLine = int.Parse(codesEnd2[0]);
                        result.EndColumn = int.Parse(codesEnd2[1]);
                        result.EndFloor = int.Parse(codesEnd2[2]);

                        break;
                    case TaskType.移库:
                        result.TaskType = PLCTaskType.移库;

                        //取货层排列
                        var codesStart3 = stockTask.OutLocationCode.Split('-');
                        result.StartLine = int.Parse(codesStart3[0]);
                        result.StartColumn = int.Parse(codesStart3[1]);
                        result.StartFloor = int.Parse(codesStart3[2]);

                        //目的层排列
                        var codesEnd3 = stockTask.InLocationCode.Split('-');
                        result.EndLine = int.Parse(codesEnd3[0]);
                        result.EndColumn = int.Parse(codesEnd3[1]);
                        result.EndFloor = int.Parse(codesEnd3[2]);

                        break;
                    case TaskType.存车修正:
                        result.TaskType = PLCTaskType.出库作业;
                        //todo取货层排列 目的层排列
                        break;
                }


                result.VerificationCode = result.TaskCode + (int)result.TaskType + result.StartLine + result.StartFloor + result.StartColumn + result.EndLine + result.EndFloor + result.EndColumn;
                result.TaskRFID = stockTask.CarTypeNum;
                var logMessage = new LogMessage()
                {

                    Content = "向" + request.EquipmentId + "号堆垛机发送" + result.TaskType.ToString() + ",任务号" + result.TaskCode + ";起始位置" + result.StartLine + "排" + result.StartColumn + "列" + result.StartFloor + "层，目标位置" + result.EndLine + "排" + result.EndColumn + "列" + result.EndFloor + "层，验证号：" + result.VerificationCode,

                    Level = LogLevel.Information,
                    Timestamp = DateTime.Now,
                };

                //await _hubContext.Clients.All.showMsg(logMessage);
                //记录到日志文件
                this._logger.LogInformation(logMessage.Content);

                return result;
            }
            else
            {
                return null;
            }




        }
    }
}
