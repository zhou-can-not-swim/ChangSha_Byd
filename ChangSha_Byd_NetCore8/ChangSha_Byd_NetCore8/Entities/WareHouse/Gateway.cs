using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using ChangSha_Byd_NetCore8.Protocols.Common;
using FutureTech.Dal.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities.WareHouse
{
    /// <summary>
    /// 库口
    /// </summary>
    [Table("Gateway")]
    public class Gateway : FutureBaseEntity<int>
    {
        public string? Code { get; set; }

        public string? Name { get; set; }


        /// <summary>
        /// 类型  初始化 = 0, 进口 = 1, 出口 = 2    
        /// </summary>
        [Description("类型")]
        public GatewayType Type { get; set; }


        /// <summary>
        /// 仓库id
        /// </summary>
        [Description("仓库id")]
        public int? WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }


        /// <summary>
        /// 所属堆垛机id
        /// </summary>
        [Description("所属堆垛机id")]
        public int EquipmentId { get; set; }

        public Equipment? Equipment { get; set; }

        public int LocationId { get; set; }

        /// <summary>
        /// 库位，库口放货取货的库位编号
        /// </summary>
        public string? LocationCode { get; set; }
    }
}
