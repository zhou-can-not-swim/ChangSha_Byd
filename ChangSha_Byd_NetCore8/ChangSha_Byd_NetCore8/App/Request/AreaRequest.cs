using ChangSha_Byd_NetCore8.OpenAuth.App.Request;

namespace Byd.Services.Request
{
  public  class AreaRequest
    {
    }
    public class QueryAreaReq: PageReq
    { 
        public int? WarehouseId { get; set; }
    
    }
    public class GetAreaListInput
    {
        public int? WarehouseId { get; set; }
        public string Key { get; set; }
    }
}
