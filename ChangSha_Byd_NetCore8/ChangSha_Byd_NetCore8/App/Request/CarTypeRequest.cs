using ChangSha_Byd_NetCore8.OpenAuth.App.Request;

namespace Byd.Services.Request
{
    public class CarTypeRequest
    {
    }
    public class QueryCarTypeReq : PageReq
    {
        public int? WarehouseId { get; set; }
    }

    public class GetCarTypeEntityInput
    { 
        /// <summary>
        /// 台车编号
        /// </summary>
    public string CarTypeNum { get; set; }
        /// <summary>
        /// 车型编码
        /// </summary>
        public string CarTypeCode { get; set; }
    }
}
