using ChangSha_Byd_NetCore8.Protocols.Common;
using FutureTech.Dal.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities.WarehouseModel
{
    /// <summary>
    /// 仓库表
    /// </summary>
    [Table("Warehouse")]
    public partial class Warehouse : FutureBaseEntity<int>
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Description("编号")]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; }
        /// <summary>
        ///类型 CW 1 QH2
        /// </summary>
        [Description("类型")]
        public WarehouseType WarehouseType { get; set; }
    }
}
