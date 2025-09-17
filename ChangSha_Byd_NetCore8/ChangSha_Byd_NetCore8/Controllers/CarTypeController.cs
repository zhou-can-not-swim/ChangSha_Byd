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
    /// 车型控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class CarTypeController : ControllerBase
    {
        private readonly CarTypeApp _carTypeApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CarTypeController(CarTypeApp carTypeApp)
        {
            _carTypeApp = carTypeApp;
        }

        [HttpGet]
        public async Task<TableData> Load([FromQuery] QueryCarTypeReq request)
        {
            return await _carTypeApp.Load(request);
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<IReadOnlyList<CarType>>> GetByQuery([FromQuery] QueryCarTypeReq input)
        {
            var result = new Response<IReadOnlyList<CarType>>();
            try
            {
                var query = await _carTypeApp.GetList(input);
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
        public async Task<Response> Add(CarType obj)
        {
            var result = new Response<CarType>();
            try
            {
                if (!await _carTypeApp.IsHaveType(obj))
                {
                    var newObj = await _carTypeApp.AddAsync(obj);
                    result.Result = newObj.Data;
                }
                else
                {
                    result.Code = 500;
                    result.Message = "该车型已存在";
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
        public async Task<Response> Update(CarType obj)
        {
            var result = new Response<CarType>();
            try
            {
                if (!await _carTypeApp.IsHaveType(obj))
                {
                    obj.Warehouse = null;
                     var res = await _carTypeApp.UpdateForTrackedAsync(obj);
                        result.Result = res.Data;
                }
                else
                {
                    result.Code = 500;
                    result.Message = "该车型已存在";
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
        public async Task<Response<string>> DeleteEntity(DeleteByIdsInput input)
        {
            var result = new Response<string>();
            try
            {
                foreach (var Id in input.Ids)
                {
                    var query = await _carTypeApp.GetByIdAsync(Id);
                    query.Warehouse = null;
                    await _carTypeApp.DeleteForTrackedAsync(query);

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
