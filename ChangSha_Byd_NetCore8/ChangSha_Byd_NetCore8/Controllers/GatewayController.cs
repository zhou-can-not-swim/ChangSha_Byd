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
    /// 库口控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly GatewayApp _gatewayApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GatewayController(GatewayApp gatewayApp)
        {
            _gatewayApp = gatewayApp;
        }

        [HttpGet]
        public async Task<TableData> Load([FromQuery]QueryGatewayReq request)
        {
            return await _gatewayApp.Load(request);
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<IReadOnlyList<Gateway>>> GetByQuery([FromQuery]GetGateWayListInput input)
        {
            var result = new Response<IReadOnlyList<Gateway>>();
            try
            {
                var query = await _gatewayApp.GetList(input);
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
        public async Task<Response> Add(Gateway obj)
        {
            var result = new Response<Gateway>();
            try
            {
                if (!await _gatewayApp.IsHaveCode(obj))
                {
                    var newObj = await _gatewayApp.AddAsync(obj);
                    result.Result = newObj.Data;
                }
                else
                {
                    result.Code = 500;
                    result.Message = "该库口已存在，请重新输入一个新的编码";
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
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response> Update(Gateway obj)
        {
            var result = new Response<Gateway>();
            try
            {
                if (!await _gatewayApp.IsHaveCode( obj))
                {
                    obj.Warehouse = null;
                obj.Equipment = null;
                var res = await _gatewayApp.UpdateForTrackedAsync(obj);
                result.Result = res.Data;
                }
                else
                {
                    result.Code = 500;
                    result.Message = "该库口已存在，请重新输入一个新的编码";
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
        /// 删除
        /// </summary>
        [HttpPost]
        public async Task<Response<string>> Delete(DeleteByIdsInput input)
        {
            var result = new Response<string>();
            try
            {
                foreach (var Id in input.Ids)
                {
                    var query = await _gatewayApp.GetByIdAsync(Id);
                    query.Warehouse = null;
                    query.Equipment = null;
                    await _gatewayApp.DeleteForTrackedAsync(query);

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
