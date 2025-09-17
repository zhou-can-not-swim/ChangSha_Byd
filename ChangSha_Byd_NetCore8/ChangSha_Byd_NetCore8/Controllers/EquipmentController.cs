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
    /// 设备控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly EquipmentApp _equipmentApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentController(EquipmentApp equipmentApp)
        {
            _equipmentApp = equipmentApp;
        }

        [HttpGet]
        public async Task<TableData> Load([FromQuery]QueryEquipmentReq request)
        {
            return await _equipmentApp.Load(request);
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<IReadOnlyList<Equipment>>> GetByQuery([FromQuery]EquipmentRequest input)

        {
            var result = new Response<IReadOnlyList<Equipment>>();
            try
            {
                var query = await _equipmentApp.GetList(input);
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
        public async Task<Response> Add(Equipment obj)
        {
            var result = new Response<Equipment>();
            try
            {
                if (!await _equipmentApp.IsHaveCode(obj.Code))
                {
                    var newObj = await _equipmentApp.AddAsync(obj);
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
        public async Task<Response> Update(Equipment obj)
        {
            var result = new Response<Equipment>();
            try
            {
                obj.Warehouse = null;
                var res = await _equipmentApp.UpdateForTrackedAsync(obj);
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
                    var query = await _equipmentApp.GetByIdAsync(Id);
                    query.Warehouse = null;
                    await _equipmentApp.DeleteForTrackedAsync(query);

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
