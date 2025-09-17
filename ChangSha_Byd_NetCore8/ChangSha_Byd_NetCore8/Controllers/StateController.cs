using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.App.WarehouseModel;
using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Byd.WebApi.Controllers
{

    /// <summary>
    /// 状态控制器
    /// </summary>
    [Route("api/wms/[controller]/[action]")]
    [ApiController]
    public class StateController:ControllerBase
    {
        private readonly StateApp _stateApp;
        private readonly IOptions<AppSetting> _appConfiguration;

        public StateController(StateApp stateApp, IOptions<AppSetting> appConfiguration)
        {
            _stateApp = stateApp;
            _appConfiguration = appConfiguration;
        }


        /// <summary>
        /// 加载所有信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TableData> Load([FromQuery] QueryStateReq request)
        {
            return await _stateApp.Load(request);
        }
        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        public async Task<Response> Add(State obj)
        {
            var result = new Response<State>();
            try
            {
                //if (!await _stateApp.IsHaveType(obj))
                //{
                 //obj.WarehouseId = 4;
                 //obj.WarehouseName = "地板库";
                 //obj.EquipmentId=1;
                 //obj.EquipmentName = "地板堆垛机";
                 //obj.Dname = "联机";
                  var newObj = await _stateApp.AddAsync(obj);
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
        /// 添加堆垛机状态信息
        /// </summary>
        [HttpPost]
        public async Task<Response> AddPLCState(AddInStateTaskInput inStateTaskInput)
        {
            //获取仓库id(只有主线库特别)
            var CangkuID = _appConfiguration.Value.WarehouseId;
            var result = new Response<State>();
            try
            {
                
              
                await _stateApp.AddInState(inStateTaskInput);

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
