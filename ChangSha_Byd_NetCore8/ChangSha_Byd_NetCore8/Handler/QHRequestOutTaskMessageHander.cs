using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Production;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request;
using MediatR;

namespace ChangSha_Byd_NetCore8.Handler
{
    /// <summary>
    ///请求发送出库任务，并且要校验RFID
    /// </summary>
    public class QHRequestOutTaskMessageHander : IRequestHandler<RequestTaskRequest, RequestTaskResponse>
    {
        private readonly StockTaskApp _stockTaskApp;

        public QHRequestOutTaskMessageHander(StockTaskApp stockTaskApp)
        {
            _stockTaskApp = stockTaskApp;
        }

        public async Task<RequestTaskResponse> Handle(RequestTaskRequest request, CancellationToken cancellationToken)
        {
            //线查询一下有没有已校验通过的出库任务
            GetRequestStockTaskEntityInput inputc = new GetRequestStockTaskEntityInput()
            {
                CarTypeNum = request.RFid,
                //Status = 6,//已校验等待出库
                Status = 6,//因为是在自动的时候,发的说明执行的任务是正在执行（因为出现有的情况，RFID会发错口子）
                TaskType = 2//出库任务,
            };

            //得到所有出库任务
            var entityc = await _stockTaskApp.GetRequstStockTaskEntity(inputc);
            if (entityc == null)//说明当前没有 已校验  出库任务？？？
            {
                QH_OutLocation outLocation = request.outLocation;
                //这是什么写法？？？
                var OutGateWayIds = typeof(QH_OutLocation).GetFields()
                .Select(a =>
                {
                    return Convert.ToInt32(a.GetValue(outLocation));
                })
                .ToList();

                GetRequestStockTaskEntityInput input = new GetRequestStockTaskEntityInput()
                {
                    Status = 1,//等待执行
                    TaskType = 2,//出库任务
                    OutGateWayIds = OutGateWayIds
                };
                //查询 出库任务 为 等待执行 的任务
                var entity = await _stockTaskApp.GetRequstStockTaskEntity(input);
                if (entity != null)//查到了 等待执行 的任务
                {
                    RequestTaskResponse result = new RequestTaskResponse();
                    result.outLocation = (QH_OutLocation)entity.GatewayId;
                    result.TaskRFID = entity.CarTypeNum;
                    //修改请求时间
                    entity.RequestTaskTime = DateTime.Now;
                    await _stockTaskApp.UpdateForNotTrackedAsync(entity, true);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //有 已校验 的出库任务，直接返回
                RequestTaskResponse results = new RequestTaskResponse();
                results.outLocation = (QH_OutLocation)entityc.GatewayId;//出库口
                results.TaskRFID = entityc.CarTypeNum;//车型编号也就是rfid
                return results;
            }

        }
    }
}
