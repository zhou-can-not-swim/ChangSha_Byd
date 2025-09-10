using ChangSha_Byd_NetCore8.OpenAuth.App.Request;


namespace Byd.Services.Request
{
    public class InventoryRequest
    {
    }
    public class InventoryDto
    {
    }

    public class QueryInventoryReq : PageReq
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? LocationId { get; set; }
        public int? CarTypeId { get; set; }
        public string carTypeNameKey { get; set; }
    }


    public class GetInventoryListInput
    {
        public string Key { get; set; }
        public List<int> Ids { get; set; }
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? NotAreaId { get; set; }
        public int? LocationId { get; set; }

        public int? CarTypeId { get; set; }
        public string CarTypeNum { get; set; }
        public string CarTypeInt { get; set; }
        public int? CarTypeFace { get; set; }
        public int? InGatewayId { get; set; }
        public int? OutGatewayId { get; set; }
        public bool? IsRepair { get; set; }
        public int? Status { get; set; }



    }

    public class GetInventoryListOutput
    {
        public int InventoryId { get; set; }
        public int WarehouseId { get; set; }

        public int AreaId { get; set; }

        public string AreaName { get; set; }

        public int LocationId { get; set; }
        public string LocationCode { get; set; }

        public int CarTypeId { get; set; }

        public string CarTypeName { get; set; }

        public string CarTypeNum { get; set; }

        public DateTime CreateTime { get; set; }
        public int Status { get; set; }

        public int SetTaskType { get; set; }

        public int? InGatewayId { get; set; }

        public string InGatewayName { get; set; }

        public int? OutGatewayId { get; set; }

        public string OutGatewayName { get; set; }
    }
    public class GetInventoryReportOutput
    {
        //public int CarTypeInt { get; set; }

        public int Count { get; set; }
        public string CarTypeName { get; set; }

       
    }

}
