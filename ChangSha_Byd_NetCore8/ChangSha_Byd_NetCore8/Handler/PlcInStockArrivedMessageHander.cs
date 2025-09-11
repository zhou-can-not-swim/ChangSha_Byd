using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Production;
using ChangSha_Byd_NetCore8.App.WarehouseModel;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.Hub;
using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Log;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChangSha_Byd_NetCore8.Handler
{
    /// <summary>
    /// 入口位有料的处理消息
    /// </summary>                                               //消息处理器需要处理的请求类型          //返回的响应类型
    public class PlcInStockArrivedMessageHander : IRequestHandler<InStockArrivedRequest, InStockArrivedResponse>
    {
        private const string V = "手动";
        private readonly StockTaskApp _stockTaskApp;
        private readonly Area_CarType_GateWayApp _area_CarType_GateWayApp;
        private readonly CarTypeApp _carTypeApp;
        private readonly LocationApp _locationApp;
        private readonly IHubContext<ProductionHub, IProductionHub> _hubContext;
        private readonly ILogger<PlcInStockArrivedMessageHander> _logger;

        public PlcInStockArrivedMessageHander(
            StockTaskApp stockTaskApp,
            Area_CarType_GateWayApp area_CarType_GateWayApp,
            CarTypeApp carTypeApp, LocationApp locationApp,
            IHubContext<ProductionHub,
            IProductionHub> hubContext,
            ILogger<PlcInStockArrivedMessageHander> logger
        )
        {
            _stockTaskApp = stockTaskApp;
            _area_CarType_GateWayApp = area_CarType_GateWayApp;
            _carTypeApp = carTypeApp;
            _locationApp = locationApp;
            _hubContext = hubContext;
            _logger = logger;
        }
        //           返回值是响应模型                     //参数请求模型
        public async Task<InStockArrivedResponse> Handle(InStockArrivedRequest request, CancellationToken cancellationToken)
        {
            //返回就返回InStockArrivedResponse类型的response数据，在入库中间件的97行接收
            InStockArrivedResponse response = new InStockArrivedResponse();
            //检测任务是否已存在,查找数据库                      （夹具库的16进制rfid，入库操作）
            var isHaveTask = await _stockTaskApp.IsHaveStockTask(request.Rfid16, TaskType.入库);
            if (!isHaveTask.Result)//如果没有任务
            {
                //1.判断台车编号_车型类型_出入口号是不是符合规则
                //1.1 如果台车编号不为空，查出对应的车型
                CarType carTypeEntity = null;
                if (!string.IsNullOrEmpty(request.Rfid16))
                {
                    //处理16进制rfid信息看看是否符合库口要求，也就要提将你的车型给记录在数据库
                    //2041有没有这个rfid对应的车型，有的话就把那个记录返回回来
                    //参数为（2041 10），10说明InStockNo=10，其实没有用到这个变量，返回CarType的实体
                    carTypeEntity = await _carTypeApp.GetCarTypeByCarTypeNum(request.Rfid16, (int)request.InStockNo);

                    //如果车型为空，说明没找到rfid在这个入库口
                    if (carTypeEntity == null)
                    {
                        response.IsSucess = false;
                        response.Msg = "未找到匹配的车型，请检查填写的台车编号是否正确！";
                        return response;
                    }
                }
                else
                {
                    response.IsSucess = false;
                    response.Msg = "请选择入库的台车编号！";
                    return response;
                }

                //1.2 判断车型_出入口号是否符合存放规则（也就是说我给的rfid 2041是符合规范的，我现在就要对我给的rfid来操作）
                GetArea_CarType_GateWayListInput getArea_CarType_GateWayListInput = new GetArea_CarType_GateWayListInput();
                getArea_CarType_GateWayListInput.CarTypeId = carTypeEntity.Id;//拿到CarType的id号（3）
                                                                              //在前面得到的carTypeEntity的id可以用来查询Area_CarType_GateWay表，目的为了匹配入口号
                List<Area_CarType_GateWay> area_CarType_GateWayList = await _area_CarType_GateWayApp.GetArea_CarType_GateWayList(getArea_CarType_GateWayListInput);
                if (area_CarType_GateWayList == null || area_CarType_GateWayList.Count == 0)//如果在Area_CarType_GateWay表中没有找到对应的CarTypeId=3的记录
                {
                    response.IsSucess = false;
                    response.Msg = "未找到该车型匹配的入口号。";
                    return response;
                }

                //找到了，开始查找入库口号InGatewayId是否匹配
                var inGateWayIds = area_CarType_GateWayList.Select(a => a.InGatewayId).ToArray();
                //如果不包含这个入库口号的话
                if (!inGateWayIds.Contains(request.InStockNo))
                {
                    response.IsSucess = false;
                    response.Msg = "该车型与选择的入口号不匹配，请选择“" + area_CarType_GateWayList[0].InGateway.Name + "”";
                    return response;
                }

                //入库口号和车型都匹配成功，获取Area_CarType_GateWay实体
                var area_CarType_GateWayEntity = area_CarType_GateWayList
                    .Where(a => a.InGatewayId == request.InStockNo && a.CarTypeId == carTypeEntity.Id)
                    .FirstOrDefault();

                //2.获取入库的库位
                var locationEntity = await _locationApp.GetInStockLocation(area_CarType_GateWayEntity);
                if (locationEntity == null)
                {
                    response.IsSucess = false;
                    response.Msg = "库位不足！";
                    return response;
                }


                //3.添加入库任务
                AddInStockTaskInput input = new AddInStockTaskInput();
                input.CarTypeNum = request.Rfid16;
                input.Remark = request.Remark;
                input.RFIDTen = request.RFIDTen;//夹具库新增10进制RFID

                //放到任务表中去
                var StockTask = await _stockTaskApp.AddInStockTask(input, carTypeEntity, area_CarType_GateWayEntity, locationEntity);
                if (StockTask != null)
                {
                    response.Result = StockTask;
                    response.IsSucess = true;
                    var logMessage2 = new LogMessage()
                    {
                        Content = "分配任务号：" + StockTask.TaskNo + ",库位" + StockTask.InLocationCode,
                        Level = LogLevel.Information,
                        Timestamp = DateTime.Now,
                    };

                    //在前端展现出来
                    await _hubContext.Clients.All.showMsg(logMessage2);
                    //记录到日志文件
                    this._logger.LogInformation(logMessage2.Content);
                }
            }
            else
            {
                response.IsSucess = false;
                response.Msg = isHaveTask.Message;
            }

            return response;
        }
    }
}
