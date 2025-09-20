using ChangSha_Byd_NetCore8.OpenAuth.App.Request;

namespace Byd.Services.Request
{
    public class GatewayRequest
    {
        public string key { get; set; }
        public int? WarehouseId { get; set; }

        public int? EquipmentId { get; set; }
    }
    public class GatewayDto
    {
    }
    public class QueryGatewayReq : PageReq
    {
        public int? WarehouseId { get; set; }

        public int? EquipmentId { get; set; }
    }
    public class GetGateWayListInput
    {
        public string? key { get; set; }
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? Type { get; set; }
        public int? EquipmentId { get; set; }
        public int? LocationId { get; set; }
    }

    public class GetGatewayEntityInput
    { 
    public int? Id { get; set; }

     public string Code { get; set; }
    }
}
