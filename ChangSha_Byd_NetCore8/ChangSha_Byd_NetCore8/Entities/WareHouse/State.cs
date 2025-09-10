using FutureTech.Dal.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities.WarehouseModel
{
    /// <summary>
    /// 堆垛机状态表
    /// </summary>
    [Table("State")]
    public partial class State : FutureBaseEntity<int>
    {

        /// <summary>
        /// 仓库id
        /// </summary>
        [Description("仓库id")]
        public int? WarehouseId { get; set; }

        public string  WarehouseName { get; set; }

        /// <summary>
        /// 所属堆垛机
        /// </summary>
        [Description("所属堆垛机")]
        public int? EquipmentId { get; set; }

        public string  EquipmentName { get; set; }

        /// <summary>
        /// 堆垛机的状态
        /// </summary>
        public string Dname { get; set; }

        /// <summary>
        /// 堆垛机的状态
        /// </summary>
        public string Dtrip { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        [Description("日志内容")]
        public string  Conent{ get; set; }

        /// <summary>
        ///时间
        /// </summary>
        [Description("时间")]
        public DateTime TaskTime { get; set; } = DateTime.Now;

    }
}
