using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.App.WarehouseModel;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Byd.WebApi.Controllers
{
    /// <summary>
    /// 仓库控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly LocationApp _locationApp;
        private readonly WarehouseApp _warehouseApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authUtil"></param>
        /// <param name="storehouseApp"></param>
        /// <param name="warehouseApp"></param>
        public WarehouseController(
             LocationApp locationApp, WarehouseApp warehouseApp
            )
        {
            _locationApp = locationApp;
            _warehouseApp = warehouseApp;
        }

        /// <summary>
        /// 登录验证加载产品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableData> Load([FromQuery] QueryWarehouseReq request)
        {
            return await _warehouseApp.Load(request);
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<IReadOnlyList<Warehouse>>> GetByQuery()
        {
            var result = new Response<IReadOnlyList<Warehouse>>();
            try
            {
                QueryWarehouseReq request = new QueryWarehouseReq();
                var query = await _warehouseApp.GetList(request);
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
        public async Task<Response> Add(Warehouse obj)
        {
            var result = new Response<Warehouse>();
            try
            {
                if (!await _warehouseApp.IsHaveCode(obj.Code))
                {
                    var newObj = await _warehouseApp.AddAsync(obj);
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
        public async Task<Response> Update(Warehouse obj)
        {
            var result = new Response<Warehouse>();
            try
            {
                var res = await _warehouseApp.UpdateForTrackedAsync(obj);
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
                    var query = await _warehouseApp.GetByIdAsync(Id);
                    await _warehouseApp.DeleteForTrackedAsync(query);

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
        [HttpPost]
        public async void ImportData()
        {
            var list = await _warehouseApp.ListAllAsync();
            foreach (var item in list)
            {
                if (item.Code == "005")
                {

                    for (var i = 1; i <= 20; i++)
                    {

                        for (var n = 1; n <= 100; n++)
                        {
                            var a = "";
                            if (i < 10)
                            {
                                a += ("0" + i + "-");
                            }
                            else
                            {
                                a += (i + "-");
                            }

                            if (n < 10)
                            {
                                a += ("00" + n);
                            }
                            else if (n == 100)
                            {
                                a += "100";
                            }
                            else
                            {
                                a += ("0" + n);
                            }

                            Location entity = new Location();
                            entity.Code = a;
                            entity.Name = a;
                            entity.WarehouseId = item.Id;
                            await _locationApp.AddAsync(entity);
                        }

                    }
                }
            }
        }
    }
}
