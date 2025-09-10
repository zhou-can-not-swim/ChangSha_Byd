using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using FutureTech.Dal.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities.WareHouse
{
    /// <summary>
    /// 库区
    /// </summary>
    [Table("Area")]
    public class Area : FutureBaseEntity<int>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        [Description("仓库id")]
        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

    }
}
