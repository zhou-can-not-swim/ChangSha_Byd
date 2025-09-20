using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Production;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.App.WarehouseModel;
using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using ChangSha_Byd_NetCore8.Protocols.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Byd.WebApi.Controllers
{

    /// <summary>
    /// 任务队列控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class StockTaskController : ControllerBase
    {
        private readonly IOptions<AppSetting> _appConfiguration;
        private readonly StockTaskApp _stockTaskApp;
        private readonly Area_CarType_GateWayApp _area_CarType_GateWayApp;
        private readonly CarTypeApp _carTypeApp;
        private readonly LocationApp _locationApp;
        private readonly InventoryApp _inventoryApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public StockTaskController(StockTaskApp stockTaskApp, Area_CarType_GateWayApp area_CarType_GateWayApp, CarTypeApp carTypeApp, LocationApp locationApp, InventoryApp inventoryApp, IOptions<AppSetting> appConfiguration)
        {
            _stockTaskApp = stockTaskApp;
            _area_CarType_GateWayApp = area_CarType_GateWayApp;
            _carTypeApp = carTypeApp;
            _locationApp = locationApp;
            _inventoryApp = inventoryApp;
            _appConfiguration = appConfiguration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<TableData> Load([FromQuery] QueryStockTaskReq request)
        {
            return await _stockTaskApp.Load(request);
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<Response<IReadOnlyList<StockTask>>> GetByQuery([FromQuery] GetStockTaskListInput input)
        {
            var result = new Response<IReadOnlyList<StockTask>>();
            try
            {
                var query = await _stockTaskApp.GetList(input);
                result.Result = query;   //返回ID
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 添加手动入库任务
        /// </summary>
        [HttpPost]
        public async Task<Response> AddInStockTask(AddInStockTaskInput input)
        {
            //获取仓库id
            var CangkuID= _appConfiguration.Value.WarehouseId;
            //前端输入台车编号，库口号，库位（可选）
            var result = new Response<StockTask>();
            try
            {
                if (input.IsRepair)
                {
                    input.Remark = "手动下发维修入库任务。";
                }
                else
                {
                    input.Remark = "手动下发入库任务。";
                    //只有主线库手动入库的时候才会出现机械编号转成plc编号
                    //判断当前是主线库还是夹具库
                    //if (CangkuID == 1)
                    //{//主线库
                    //    var JxCarTypeNumber = await _stockTaskApp.ShouDongRukuRfid(input.CarTypeNum,(int)input.GatewayId);
                    //    if (JxCarTypeNumber!=""||JxCarTypeNumber!=null)
                    //    {
                    //        input.CarTypeNum= JxCarTypeNumber;
                    //    }
                    //}

                }
                //检测任务是否已存在
                var isHaveTask = await _stockTaskApp.IsHaveStockTask(input.CarTypeNum, TaskType.入库);
                if (!isHaveTask.Result)
                {
                    //1.判断台车编号_车型类型_出入口号是不是符合规则
                    //1.1 如果台车编号不为空，查出对应的车型
                    CarType carTypeEntity = null;
                    if (!string.IsNullOrEmpty(input.CarTypeNum))
                    {
                        
                        carTypeEntity = await _carTypeApp.GetCarTypeByCarTypeNum(input.CarTypeNum,(int)input.GatewayId);

                        if (carTypeEntity == null)
                        {
                            result.Code = 500;
                            result.Message = "未找到匹配的车型，请检查填写的台车编号是否正确！";
                            return result;
                        }
                    }
                    else
                    {
                        result.Code = 500;
                        result.Message = "请选择入库的台车编号！";
                        return result;
                    }

                    //1.2 判断车型_出入口号是否符合存放规则
                    GetArea_CarType_GateWayListInput getArea_CarType_GateWayListInput = new GetArea_CarType_GateWayListInput();
                    getArea_CarType_GateWayListInput.CarTypeId = input.CarTypeId;
                    getArea_CarType_GateWayListInput.IsRepair = input.IsRepair;
                    List<Area_CarType_GateWay> area_CarType_GateWayList = await _area_CarType_GateWayApp.GetArea_CarType_GateWayList(getArea_CarType_GateWayListInput);
                    if (area_CarType_GateWayList == null || area_CarType_GateWayList.Count == 0)
                    {
                        result.Code = 500;
                        result.Message = "未找到该车型匹配的入口号!";
                        return result;
                    }
                    var inGateWayIds = area_CarType_GateWayList.Select(a => a.InGatewayId).ToArray();
                    if (!inGateWayIds.Contains((int)input.GatewayId))
                    {
                        result.Code = 500;
                        result.Message = "该车型与选择的入口号不匹配，请选择“" + area_CarType_GateWayList[0].InGateway.Name+ "”!";
                        return result;
                    }
                    var area_CarType_GateWayEntity = area_CarType_GateWayList.Where(a => a.InGatewayId == input.GatewayId && a.CarTypeId == carTypeEntity.Id).FirstOrDefault();
                    if (area_CarType_GateWayEntity != null)
                    {
                        //2.如果库位指定，判断库区和库位是否符合存放规则
                        Location locationEntity = null;
                        if (input.LocationId != null)
                        {
                            GetLocationInput getLocationInput = new GetLocationInput();
                            getLocationInput.Id = input.LocationId;
                            locationEntity = await _locationApp.GetLocation(getLocationInput);
                            if (locationEntity != null && area_CarType_GateWayEntity != null && locationEntity.AreaId == area_CarType_GateWayEntity.AreaId)
                            {
                            }
                            else
                            {
                                result.Code = 500;
                                result.Message = "该台车编号不能放入选择的库位，请重新选择！";
                                return result;
                            }
                        }
                        else//用户未指定库位，需要系统分配库位
                        {
                            locationEntity = await _locationApp.GetInStockLocation(area_CarType_GateWayEntity);
                             if (locationEntity == null)
                             {
                                result.Code = 500;
                                result.Message = "库位不足！";
                                return result;
                             }
                        }

                        //3.新增手工添加入库任务
                        result.Result = await _stockTaskApp.AddInStockTask(input, carTypeEntity, area_CarType_GateWayEntity, locationEntity);
                    }
                    else
                    {
                        result.Code = 500;
                        result.Message = "该车型与选择的入口号不匹配！";
                        return result;
                    }
                }
                else
                {
                    result.Code = 500;
                    result.Message = isHaveTask.Message;
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;

        }


        /// <summary>
        /// 添加手动出库任务
        /// </summary>
        [HttpPost]
        public async Task<Response> AddOutStockTask(AddOutStockTaskInput input)
        {
            var result = new Response<bool>();

            GetInventoryListInput getInventoryListInput = new GetInventoryListInput();
            getInventoryListInput.Ids = input.InventoryIds;
            var inventoryList = await _inventoryApp.GetByQuery(getInventoryListInput);
            if (inventoryList != null && inventoryList.Count > 0)
            {
                result.Result = await _stockTaskApp.AddOutStockTask(inventoryList, input.IsRepair,true);
            }
            else
            {
                result.Code = 500;
                result.Message = "库存不足！";
                return result;
            }
            return result;
        }

        /// <summary>
        /// 添加出库任务 -> 提供给MES的接口
        /// </summary>  
        [HttpPost]
        [AllowAnonymous]
        public async Task<Response> AddOutStockTaskToMes(AddOutStockTaskToMesInput input)
        {
            
            return await _stockTaskApp.AddOutStockTaskToMes(input);
        }

        ///// <summary>
        ///// 新增的
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<Response> GetPLCState()
        //{
        //    return await _stockTaskApp.GetPlcStateToMes();
        //}


        /// <summary>
        /// 添加手动移库任务
        /// </summary>
        [HttpPost]
        public async Task<Response> AddRemoveStockTask(AddRemoveStockTaskInput input)
        {
            var result = new Response<bool>();

            GetInventoryListInput getInventoryListInput = new GetInventoryListInput();
            getInventoryListInput.Ids = input.InventoryIds;
            var inventoryList = await _inventoryApp.GetByQuery(getInventoryListInput);
            if (inventoryList != null && inventoryList.Count > 0)
            {
                result.Result = await _stockTaskApp.AddRemoveStockTask(inventoryList[0], input.RemoveCount);
            }
            else
            {
                result.Code = 500;
                result.Message = "库存不足！";
                return result;
            }
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response> Update(StockTask obj)
        {
            var result = new Response<StockTask>();
            try
            {
                var res = await _stockTaskApp.UpdateForTrackedAsync(obj);
                result.Result = res.Data;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        [HttpPost]
        public async Task<Response<string>> DeleteEntity(DeleteByIdsInput input)
        {
            var result = new Response<string>();
            try
            {
                foreach (var Id in input.Ids)
                {
                    var query = await _stockTaskApp.GetByIdAsync(Id);
                    await _stockTaskApp.DeleteForTrackedAsync(query);

                }
                result.Message = "操作成功!";
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
        /// <summary>
        /// 更新任务的优先级
        /// </summary>
        [HttpPost]
        public async Task<Response<string>> UpdatePriority(UpdatePriorityInput input)
        {
            var result = new Response<string>();
            try
            {

                var res = await _stockTaskApp.UpdatePriority(input);
                result.Message = "操作成功!";
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
        /// <summary>
        /// 更新任务的状态
        /// </summary>
        [HttpPost]
        public async Task<Response<string>> UpdateTaskStatus(UpdateTaskStatusInput input)
        {
            var result = new Response<string>();
            try
            {
                var res = await _stockTaskApp.UpdateTaskStatus(input);
                if (res)
                {
                    result.Message = "操作成功!";
                }
                else
                {
                    result.Code = 500;
                    result.Message = "操作失败!";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
        /// <summary>
        ///添加移库任务。 任务已执行，执行完需要回退到原来位置，一般用在发生任务异常时使用
        /// </summary>
        [HttpPost]
        public async Task<Response> FinishedTaskAndAddMoveTask(FinishedTaskAndAddMoveTaskInput input)
        {
            var result = new Response<string>();
            try
            {
              var TaskEntity=  await _stockTaskApp.GetByIdAsNoTrackingAsync(input.TaskId);
                if (TaskEntity != null)
                {
                    if (TaskEntity.StartTime!=null)
                    {
                        var res = await _stockTaskApp.FinishedTaskAndAddMoveTask(TaskEntity);
                        if (res)
                        {
                            result.Message = "操作成功!";
                        }
                        else
                        {
                            result.Code = 500;
                            result.Message = "操作失败!";
                        }
                    
                    }
                    else
                    {
                        result.Code = 500;
                        result.Message = "当前任务还未执行，不能进行此操作！";
                    }
                 
                }
                else
                {
                    result.Code = 500;
                    result.Message = "系统未找到当前这个任务！";


                }

                return result;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }



    }
}
