using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Production;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Byd.WebApi.Controllers
{
    /// <summary>
    /// 出入库记录控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class StockTaskHistoryController :   ControllerBase
    {
        //private readonly StockTaskHistoryApp _stockTaskHistoryApp;

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        //public StockTaskHistoryController(StockTaskHistoryApp stockTaskHistorykApp)
        //{
        //    _stockTaskHistoryApp = stockTaskHistorykApp;
        //       }

        //[HttpGet]
        //public async Task<TableData> Load([FromQuery] QueryStockTaskHistoryReq request)
        //{
        //    return await _stockTaskHistoryApp.Load(request);
        //}

        //[HttpGet]
        //public async Task<Response<GetTongJiTaskHistroyOutput>> GetTongJiTaskHistroy()
        //{
        //    Response<GetTongJiTaskHistroyOutput> result = new Response<GetTongJiTaskHistroyOutput>() { Result = await _stockTaskHistoryApp.GetTongJiTaskHistroy() };
        //    return result;

        //}

        ///// <summary>
        ///// 饼图中的统计数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<Response<GetTongJiDayCount>> GetTongJiTaskDayHistroy()
        //{
        //    Response<GetTongJiDayCount> result = new Response<GetTongJiDayCount>() { Result = await _stockTaskHistoryApp.GetTongJiTaskDayHistroy() };
        //    return result;

        //}

    }
}
