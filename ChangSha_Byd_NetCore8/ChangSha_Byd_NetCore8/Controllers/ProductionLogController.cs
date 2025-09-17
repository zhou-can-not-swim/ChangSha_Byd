using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Byd.WebApi.Controllers
{
    /// <summary>
    /// 生产日志控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class ProductionLogController : ControllerBase
    {
        //private readonly ProductionLogApp _productionLogApp;

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        //public ProductionLogController(ProductionLogApp productionLogApp)
        //{
        //    _productionLogApp = productionLogApp;
        //}

        //[HttpGet]
        //public async Task<TableData> Load([FromQuery] QueryProductionLogReq request)
        //{
        //    return await _productionLogApp.Load(request);
        //}
    }
}
