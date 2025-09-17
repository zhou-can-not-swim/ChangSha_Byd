using ChangSha_Byd_NetCore8.Controllers.Dto;
using ChangSha_Byd_NetCore8.Extends;
using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.Protocols.QHStocker;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ChangSha_Byd_NetCore8.Controllers
{
    [Route("/api/db")]
    [ApiController]
    public class ZBotuController : ControllerBase
    {
        private readonly IServiceScopeFactory _ssf;
        private readonly IMediator _mediator;
        private readonly ILogger<PlcHostedService> _logger;
        private readonly PlcMgr _plcmgr;
        private readonly IOptionsMonitor<ScanOpts> _scanOptionsMonitor;
        private readonly ScanContext context;
        private readonly IMemoryCache _cache;

        public ZBotuController(IServiceScopeFactory ssf, 
            IMediator mediator, ILogger<PlcHostedService> logger, PlcMgr plcmgr, IOptionsMonitor<ScanOpts> scanOpts,
            IMemoryCache cache)
        {
            _ssf = ssf;
            _mediator = mediator;
            _plcmgr = plcmgr;
            _scanOptionsMonitor = scanOpts;
            _cache = cache;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<PlcMsg> Load()
        {
            //读plc给上位机部分
            var plcOp = await _plcmgr.PlcName_QHStocker.ReadPlcMsgAsync();
            var plcmsg = plcOp.ResultValue;//plc信息
            //对得到的plc信息进行处理



            ////读上位机写的部分
            //var mstOp = await _plcmgr.PlcName_CWStocker.ReadMstMsgAsync();
            return plcmsg;
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<Object> ChangeAsync([FromBody]Object conetxt)
        //{
        //    return conetxt;
        //    //var v = _cache.Set<ScanContext>("conetxt", conetxt);
        //    //return conetxt;
        //}

        // POST /api/db
        [HttpPost]
        [AllowAnonymous]
        public IActionResult PostAll([FromBody]MstDto dto)
        {

            Console.WriteLine(dto.SendTaskReq);
            Console.WriteLine(dto.FinishTaskAck);


            return Ok();
        }




        //[HttpGet("change")]
        //[AllowAnonymous]
        //public async Task<FSharpResult<ValueTuple, ApiError>> SetPlcMsg(
        //)
        //{
        //    // 创建 MstMsg 实例
        //    var msg = new MstMsg();

        //    // 只发送心跳请求
        //    msg.GeneralCmdWord = MstMsg.MstFlags_GeneralCmdWord.心跳请求;

        //    // 设置预留1（假设这是一个字节类型的预留字段）
        //    msg.预留1 = 0x01; // 你可以设置任何你需要的值

        //    await _plcmgr.PlcName_QHStocker.SendCmdAsync(msg);






        //}

    }

}
