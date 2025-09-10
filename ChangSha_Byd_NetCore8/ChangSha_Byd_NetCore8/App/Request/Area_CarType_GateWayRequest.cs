using ChangSha_Byd_NetCore8.OpenAuth.App.Request;

namespace Byd.Services.Request
{
    public class Area_CarType_GateWayRequest
    {
    }

    public class QueryArea_CarType_GateWayReq : PageReq
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public string Key { get; set; }
        public int? CarTypeId { get; set; }

        public int? InGateWayId { get; set; }
        public int? OutGateWayId { get; set; }

    }
    /// <summary>
    /// 区域、车辆类型、出入口
    /// </summary>
    public class GetArea_CarType_GateWayListInput
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public string Key { get; set; }
        public int? CarTypeId { get; set; }

        public int? InGateWayId { get; set; }
        public int? OutGateWayId { get; set; }
        public bool? IsRepair { get; set; }
    }
    public class GetArea_CarType_GateWayInput
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public string Key { get; set; }
        public int? CarTypeId { get; set; }

        public int? InGateWayId { get; set; }
        public int? OutGateWayId { get; set; }
    }
}
