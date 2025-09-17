using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.App.WarehouseModel;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Byd.WebApi.Controllers
{

    /// <summary>
    /// 出入规则控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class Area_CarType_GateWayController : ControllerBase
    {
        private readonly Area_CarType_GateWayApp _area_CarType_GateWayApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Area_CarType_GateWayController(Area_CarType_GateWayApp area_CarType_GateWayApp)
        {
            _area_CarType_GateWayApp = area_CarType_GateWayApp;
        }

        [HttpGet]
        public async Task<TableData> Load([FromQuery] QueryArea_CarType_GateWayReq request)
        {
            return await _area_CarType_GateWayApp.Load(request);
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<Area_CarType_GateWay>> GetByQuery([FromQuery] GetArea_CarType_GateWayListInput input)
        {
            var result = new Response<Area_CarType_GateWay>();
            try
            {
                var query = await _area_CarType_GateWayApp.GetArea_CarType_GateWayList(input);
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
        /// 添加
        /// </summary>
        [HttpPost]
        public async Task<Response> Add(Area_CarType_GateWay obj)
        {
            var result = new Response<Area_CarType_GateWay>();
            try
            {
                obj.Area = null;
                obj.InGateway = null;
                obj.OutGateway = null;
                obj.CarType = null;
                var newObj = await _area_CarType_GateWayApp.AddAsync(obj);
                result.Result = newObj.Data;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response> Update(Area_CarType_GateWay obj)
        {
            var result = new Response<Area_CarType_GateWay>();
            try
            {
                obj.Area = null;
                obj.InGateway = null;
                obj.OutGateway = null;
                obj.CarType = null;
                var res = await _area_CarType_GateWayApp.UpdateForTrackedAsync(obj);
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
                    var obj = await _area_CarType_GateWayApp.GetByIdAsync(Id);
                    obj.Area = null;
                    obj.InGateway = null;
                    obj.OutGateway = null;
                    obj.CarType = null;
                    await _area_CarType_GateWayApp.DeleteForTrackedAsync(obj);

                }
                result.Message = "操作成功";
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
