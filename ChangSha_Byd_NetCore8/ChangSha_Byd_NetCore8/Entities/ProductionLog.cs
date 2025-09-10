using FutureTech.Dal.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities
{

    /// <summary>
    ///生产日志表 
    /// </summary>
    [Table("ProductionLog")]
    public class ProductionLog : FutureBaseEntity<int>
    {
        /// <summary>
        /// 仓库id
        /// </summary>
        [Description("仓库id")]
        public int WarehouseId { get; set; }
        /// <summary>
        /// 消息来源
        /// </summary>
        [Description("消息来源")]
        public string EventSource { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [Description("日志内容")]
        public string Msg { get; set; }
        /// <summary>
        /// 日志类型 1正常；2错误信息
        /// </summary>
        [Description("日志类型")]
        public int LogType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }


    }

}
