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
    /// 库位控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationApp _locationApp;
        private readonly WarehouseApp _warehouseApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authUtil"></param>
        /// <param name="locationApp"></param>
        /// <param name="warehouseApp"></param>
        public LocationController(
        //IAuth authUtil,
        LocationApp locationApp,
        WarehouseApp warehouseApp
        )
        {
            _locationApp = locationApp;
            _warehouseApp = warehouseApp;
        }
        /// <summary>
        /// 以列表的形式返回数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableData> Load([FromQuery] QueryLocationReq request)
        {
            return await _locationApp.Load(request);
        }

        /// <summary>
        /// 获取库位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<GetBoardOutput>> GetLocation([FromQuery] int? line)
        {
            Response<GetBoardOutput> result = new Response<GetBoardOutput>()
            {
                Result = await _locationApp.GetLocationFloor(line)
            };
            return result;
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<IReadOnlyList<Location>>> GetByQuery()
        {
            var result = new Response<IReadOnlyList<Location>>();
            try
            {
                QueryLocationReq queryLocationReq = new QueryLocationReq();
                var query = await _locationApp.GetList(queryLocationReq);
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
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<IReadOnlyList<Location>>> GetAllList([FromQuery] QueryLocationReq input)
        {
            Response<IReadOnlyList<Location>> result = new Response<IReadOnlyList<Location>>() { Result = await _locationApp.GetList(input) };
            return result;


        }



        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        public async Task<Response> Add(Location obj)
        {
            var result = new Response<Location>();
            try
            {
                if (!await _locationApp.IsHaveCode(obj.Code))
                {
                    var newObj = await _locationApp.AddAsync(obj);
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
        public async Task<Response> Update(Location obj)
        {
            var result = new Response<Location>();
            try
            {
                obj.Warehouse = null;
                obj.Equipment = null;
                //obj.InGateway= null;
                //obj.OutGateway = null;
                obj.Area = null;
                var res = await _locationApp.UpdateForTrackedAsync(obj);
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
                    var obj = await _locationApp.GetByIdAsync(Id);
                    obj.Warehouse = null;
                    obj.Equipment = null;
                    //obj.InGateway = null;
                    //obj.OutGateway = null;
                    obj.Area = null;
                    await _locationApp.DeleteForTrackedAsync(obj);

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



        /// <summary>
        /// 提供给看板立库库位情况视图
        /// </summary>
        [HttpGet]
        public async Task<Response<GetBoardOutput>> GetBoard([FromQuery] GetBoardInput input)
        {
            Response<GetBoardOutput> result = new Response<GetBoardOutput>() { Result = await _locationApp.GetBoard(input) };
            return result;


        }

        /// <summary>
        /// 提供给看板立库库位情况视图
        /// </summary>
        [HttpGet]
        public async Task<Response<GetLocataionCountOutput>> GetLocataionCount()
        {
            Response<GetLocataionCountOutput> result = new Response<GetLocataionCountOutput>() { Result = await _locationApp.GetLocataionCount() };
            return result;


        }

        /// <summary>
        /// 提供给看板立库库口(只查询是库口的)
        /// </summary>
        [HttpGet]
        public async Task<Response<Getkukuoutput>> GetKukou([FromQuery] Getkukou input)
        {
            Response<Getkukuoutput> result = new Response<Getkukuoutput>() { Result = await _locationApp.GetLocationKuKouList(input) };
            return result;
        }


    }
}
