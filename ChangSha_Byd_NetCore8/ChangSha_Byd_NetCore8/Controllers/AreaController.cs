using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using Microsoft.AspNetCore.Mvc;

using ChangSha_Byd_NetCore8.App.WarehouseModel;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;


namespace Byd.WebApi.Controllers
{
    /// <summary>
    /// 库区控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly AreaApp _areaApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AreaController(AreaApp areaApp)
        {
            _areaApp = areaApp;
        }

        [HttpGet]
        public async Task<TableData> Load([FromQuery]QueryAreaReq request)
        {
            return await _areaApp.Load(request);
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<IReadOnlyList<Area>>> GetByQuery([FromQuery]GetAreaListInput input)
        {
            var result = new Response<IReadOnlyList<Area>>();
            try
            {
                var query = await _areaApp.GetList(input);
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
        public async Task<Response> Add(Area obj)
        {
            var result = new Response<Area>();
            try
            {
                if (!await _areaApp.IsHaveCode(obj.Code))
                {
                    var newObj = await _areaApp.AddAsync(obj);
                    result.Result = newObj.Data;
                }
                else
                {
                    result.Code = 500;
                    result.Message = "该编号已存在，请重新输入一个新的编码";
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
        public async Task<Response> Update(Area obj)
        {
            var result = new Response<Area>();
            try
            {
                obj.Warehouse = null;
                var res = await _areaApp.UpdateForTrackedAsync(obj);
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
                    var query = await _areaApp.GetByIdAsync(Id);
                    query.Warehouse = null;
                    await _areaApp.DeleteForTrackedAsync(query);

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
