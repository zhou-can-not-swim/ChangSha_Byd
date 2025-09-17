using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Production;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Byd.WebApi.Controllers
{

    /// <summary>
    /// 库存控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryApp _iventoryApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InventoryController(InventoryApp iventoryApp)
        {
            _iventoryApp = iventoryApp;
        }

        [HttpGet]
        public async Task<TableData> Load([FromQuery] QueryInventoryReq request)
        {
            return await _iventoryApp.Load(request);
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        [HttpGet]
        public async Task<Response<IReadOnlyList<Inventory>>> GetByQuery([FromQuery] GetInventoryListInput input)
        {
            var result = new Response<IReadOnlyList<Inventory>>();
            try
            {
                var query = await _iventoryApp.GetByQuery(input);
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
        /// 查询库存
        /// </summary>
        [HttpGet]
        public async Task<Response<List<GetInventoryListOutput>>> GetInventoryList([FromQuery] GetInventoryListInput input)
        {
            var result = new Response<List<GetInventoryListOutput>>();
            try
            {
                var query = await _iventoryApp.GetInventoryList(input);
                result.Result = query;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 查询立库某一个列的库存情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<GetBoardCeOutput>>> GetBoardCe([FromQuery] GetBoardCeInput input)
        {
            var result = new Response<List<GetBoardCeOutput>>();
            try
            {
                var query = await _iventoryApp.GetBoardCe(input);
                result.Result = query;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }
        [HttpGet]
        public async Task<Response<Inventory>> GetInventoryEntity([FromQuery] GetInventoryListInput input)
        {
            var result = new Response<Inventory>();
            try
            {
                var query = await _iventoryApp.GetInventoryEntity(input);
                result.Result = query;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        [HttpGet]
        public async Task<Response<List<GetInventoryReportOutput>>> GetInventoryReport()
        {
            var result = new Response<List<GetInventoryReportOutput>>();
            try
            {
                var query = await _iventoryApp.GetInventoryReport();
                result.Result = query;
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
