using ChangSha_Byd_NetCore8.OpenAuth.App.Request;
using System;

namespace Byd.Services.Request
{
    public class StateRequest
    {
    }
    public class QueryStateReq : PageReq
    {


    }
    public class AddInStateTaskInput
    {
        public int? WarehouseId { get; set; }

        public string WarehouseName { get; set; }

    
        public int? EquipmentId { get; set; }

        public string EquipmentName { get; set; }

        public string Dname { get; set; }

      
        public string Dtrip { get; set; }

        public string Conent { get; set; }
        public DateTime TaskTime { get; set; } 
    }
}
