using ChangSha_Byd_NetCore8.Protocols.Common;
using FutureTech.Dal.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TaskStatus = ChangSha_Byd_NetCore8.Protocols.Common.TaskStatus;

namespace ChangSha_Byd_NetCore8.Entities
{
    /// <summary>
    /// 任务队列
    /// </summary>
    [Table("StockTaskHistory")]
    public class StockTaskHistory : FutureBaseEntity<int>
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [Description("流水号")]
        public string Code { get; set; }
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
        /// <summary>
        ///作业类型        
        /// </summary>
        [Description("作业类型")]
        public TaskType TaskType { get; set; }
        /// <summary>
        ///是否是维修        
        /// </summary>
        [Description("是否是维修")]
        public bool IsRepair { get; set; } = false;
        /// <summary>
        /// 状态     
        /// </summary>
        [Description("状态")]
        public TaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 优先级 值越大优先级越高
        /// </summary>
        [Description("优先级")]
        public TaskPriority Priority { get; set; }
        /// <summary>
        /// 台车编号
        /// </summary>
        [Description("台车编号")]
        public string CarTypeNum { get; set; }
        /// <summary>
        /// AB面  A面是1  B面是2
        /// </summary>
        [Description("AB面")]
        public int? CarTypeFace { get; set; } = 0;
        /// <summary>
        /// 车型
        /// </summary>
        [Description("车型")]
        public int CarTypeId { get; set; }
        /// <summary>
        /// 车型名称
        /// </summary>
        [Description("车型名称")]
        public string CarTypeName { get; set; }
        /// <summary>
        /// 车型编号
        /// </summary>
        [Description("车型编号/风车类型")]
        public string CarTypeInt { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// 库位类型
        /// </summary>
        [Description("库位类型")]
        public LocationType LocationType { get; set; }

        /// <summary>
        /// 库口
        /// </summary>
        [Description("库口")]
        public int? GatewayId { get; set; }
        /// <summary>
        /// 库口名称
        /// </summary>
        [Description("库口名称")]
        public string GatewayName { get; set; }

        /// <summary>
        /// 堆垛机号
        /// </summary>
        [Description("堆垛机号")]
        public int? EquipmentId { get; set; }

        public string EquipmentName { get; set; }

        /// <summary>
        /// 作业任务号
        /// </summary>
        [Description("作业任务号")]
        public int TaskNo { get; set; }


        /// <summary>
        /// 取库位
        /// </summary>
        [Description("取库位")]
        public int? OutLocationId { get; set; }

        public string OutLocationCode { get; set; }
        /// <summary>
        /// 放库位
        /// </summary>
        [Description("放库位")]
        public int? InLocationId { get; set; }

        public string InLocationCode { get; set; }


        /// <summary>
        ///完成方式自动=1,手动=0 
        /// </summary>
        [Description("完成方式")]
        public int SetTaskType { get; set; } = 1;

        /// <summary>
        /// 报警内容
        /// </summary>
        [Description("报警内容")]
        public string WarnContent { get; set; }

        /// <summary>
        /// 分拼请求校验RFID时间
        /// </summary>
        [Description("分拼请求校验RFID时间")]
        public DateTime RequestTaskTime { get; set; } = DateTime.Now;
        /// <summary>
        ///机械编号
        /// </summary>
        [Description("plc编号转机械编号")]
        public string JXCarTypeNum { get; set; }

    }

}
