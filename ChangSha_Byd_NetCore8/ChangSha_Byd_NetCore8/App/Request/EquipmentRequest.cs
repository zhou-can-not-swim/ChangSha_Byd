using ChangSha_Byd_NetCore8.OpenAuth.App.Request;

namespace Byd.Services.Request
{
  public  class EquipmentRequest
    {
        public int? WarehouseId { get; set; }
        public string key { get; set; }
    }

    public class EquipmentDto
    {
    }

    public class QueryEquipmentReq : PageReq
    {
        public int? WarehouseId { get; set; }
    }

    public class GetEquementUsed
    {
        public int? WarehouseId { get; set;}
        public bool? IsUsed { get; set;}
    }

}
