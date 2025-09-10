using FutureTech.Dal.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities
{

    /// <summary>
    ///库存表 
    /// </summary>
    [Table("Inventory")]
    public class Inventory : FutureBaseEntity<int>
    {
        /// <summary>
        /// 仓库id
        /// </summary>
        [Description("仓库id")]
        public int WarehouseId { get; set; }

        /// <summary>
        /// 区域id
        /// </summary>
        [Description("区域id")]
        public int AreaId { get; set; }

        public string AreaName { get; set; }

        public int LocationId { get; set; }
        /// <summary>
        /// 库位id
        /// </summary>
        [Description("库位编号层排列")]
        public string LocationCode { get; set; }

        public int? CarTypeId { get; set; }
        /// <summary>
        /// 车型名称
        /// </summary>
        [Description("车型名称")]
        public string CarTypeName { get; set; }
        /// <summary>
        /// 台车编号
        /// </summary>
        [Description("台车编号")]
        public string CarTypeNum { get; set; }
        /// <summary>
        /// AB面  A面是1  B面是2 注意：如果主线库，主线区域存1，ub区域存2
        /// </summary>
        [Description("AB面")]
        public int? CarTypeFace { get; set; } = 0;
        /// <summary>
        /// 车型编号(对应excel里的车型编号)
        /// </summary>
        [Description("车型编号/风车类型")]
        public string CarTypeInt { get; set; }

        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 1正常库存  2即将出库
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///完成方式  自动=0,手动=1
        /// </summary>
        [Description("完成方式")]
        public int SetTaskType { get; set; } = 0;

        /// <summary>
        ///机械编号
        /// </summary>
        [Description("plc编号转机械编号")]
        public string JXCarTypeNum { get; set; }
    }

}

