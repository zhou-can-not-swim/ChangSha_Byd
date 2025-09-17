using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using ChangSha_Byd_NetCore8.Protocols.Common;
using FutureTech.Dal.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities.WareHouse
{
    /// <summary>
    /// 库位
    /// </summary>
    [Table("Location")]
    public class Location : FutureBaseEntity<int>
    {
        /// <summary>
        /// 线 A/B线 A1 B2
        /// </summary>
        [Description("线 A/B线")]
        public LocationLine Line { get; set; }
        /// <summary>
        /// 库位号  排-列-层
        /// </summary>
        [Description("库位号  排-列-层")]
        public string? Code { get; set; }

        public string? Name { get; set; }
       

        /// <summary>
        /// 仓库id
        /// </summary>
        [Description("仓库id")]
        public int? WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }

        /// <summary>
        /// 库区id
        /// </summary>
        [Description("库区id")]
        public int AreaId { get; set; }

        public Area? Area { get; set; }
              /// <summary>
        /// 库位长度
        /// </summary>
        [Description("库位长度")]
        public decimal? Length { get; set; }
        /// <summary>
        /// 库位宽度
        /// </summary>
        [Description("库位宽度")]
        public decimal? Width { get; set; }
        /// <summary>
        /// 库位高度
        /// </summary>
        [Description("库位高度")]
        public decimal? Height { get; set; }

        /// <summary>
        /// 库位载重
        /// </summary>
        [Description("库位载重")]
        public decimal? Weight { get; set; }

        /// <summary>
        /// 排号
        /// </summary>
        [Description("排号")]
        public int LineNo { get; set; }
        /// <summary>
        /// 列号
        /// </summary>
        [Description("列号")]
        public int ColumnNo { get; set; }
        /// <summary>
        /// 层号
        /// </summary>
        [Description("层号")]
        public int FloorNo { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型")]
        public LocationType LocationType { get; set; }


        /// <summary>
        /// 库位状态
        /// </summary>
        [Description("库位状态")]
        public LocationStatus LocationStatus { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [Description("是否禁用")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 所属堆垛机
        /// </summary>
        [Description("所属堆垛机")]
        public int? EquipmentId { get; set; }

        public Equipment Equipment { get; set; }



    }


}
