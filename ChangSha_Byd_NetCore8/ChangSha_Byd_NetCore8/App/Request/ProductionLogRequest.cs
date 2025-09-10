using ChangSha_Byd_NetCore8.OpenAuth.App.Request;

namespace Byd.Services.Request
{
   public class ProductionLogRequest
    {
    }

    public class QueryProductionLogReq : PageReq
    {
        public int? WarehouseId { get; set; }

        public int? EquipmentId { get; set;}

        public string BeginTime { get; set; }

        public string EndTime { get; set; }

        public int? LogType { get; set; }

    }
}
