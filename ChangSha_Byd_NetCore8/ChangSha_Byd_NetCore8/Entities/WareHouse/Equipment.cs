using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using FutureTech.Dal.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities.WareHouse
{
    /// <summary>
    /// 设备
    /// </summary>
    [Table("Equipment")]
    public class Equipment : FutureBaseEntity<int>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 通讯协议TCP、OPC、Modbus
        /// </summary>
        [Description("通讯协议TCP、OPC、Modbus")]
        public string Communication { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [Description("IP地址")]
        public string IpAddress { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        [Description("端口号")]
        public string Port { get; set; }
        /// <summary>
        /// 设备状态 0故障  1正常
        /// </summary>
        [Description("设备状态")]
        public int Status { get; set; } = 1;
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        public string Picture { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Description("是否启用")]
        public bool IsUsed { get; set; } = true;

        /// <summary>
        /// 工作模式 1自动模式  2手动模式
        /// </summary>
        [Description("工作模式")]
        public int WorkModel { get; set; } = 1;

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }


    }

   

}
