using FutureTech.Dal.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities.WareHouse
{
    /// <summary>
    /// 库区-车型_出入口关系表
    /// </summary>
    [Table("Area_CarType_GateWay")]
    public class Area_CarType_GateWay : FutureBaseEntity<int>
    {
        public int WarehouseId { get; set; }

        /// <summary>
        /// 库区id
        /// </summary>
        [Description("库区id")]
        public int AreaId { get; set; }

        public Area Area { get; set; }
        /// <summary>
        /// 车型类型
        /// </summary>
        [Description("车型类型")]
        public int? CarTypeId { get; set; }
        public CarType CarType { get; set; }
        /// <summary>
        /// 入口
        /// </summary>
        [Description("入口")]
        public int? InGatewayId { get; set; }

        public Gateway InGateway { get; set; }

        /// <summary>
        /// 出口
        /// </summary>
        [Description("出口")]
        public int? OutGatewayId { get; set; }

        public Gateway OutGateway { get; set; }
        /// <summary>
        /// 是否是维修
        /// </summary>
        [Description("是否是维修")]
        public bool IsRepair { get; set; } = false;

       
    }
}
