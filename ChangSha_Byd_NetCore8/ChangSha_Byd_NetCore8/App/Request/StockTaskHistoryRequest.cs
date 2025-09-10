using ChangSha_Byd_NetCore8.OpenAuth.App.Request;
using System;

namespace Byd.Services.Request
{
  public   class StockTaskHistoryRequest
    {
    }
    public class QueryStockTaskHistoryReq : PageReq
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? TrayId { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }

        public string ProductId { get; set; }

        public int? NotStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CarTypeNameKey { get; set; }
    }
}
