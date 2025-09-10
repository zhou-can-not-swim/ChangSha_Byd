using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Request;
using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request;
using static Azure.Core.HttpHeader;

namespace Byd.Services.Request
{
    public class StockTaskRequest
    {
    }
    public class StockTaskDto
    {
    }
    public class QueryStockTaskReq : PageReq
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? TrayId { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }

        public string ProductId { get; set; }

        public int? NotStatus { get; set; }
    }
    public class GetStockTaskListInput
    {
        public string key { get; set; }
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? TrayId { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }

        public string ProductId { get; set; }

        public int? NotStatus { get; set; }
        public int? EquipmentId { get; set; }

    }


    public class GetMBStockTaskEntityInput
    {
        public int? AreaId { get; set; }
        public ChangSha_Byd_NetCore8.Protocols.Common.TaskStatus TaskStatus { get; set; }
        public int? EquipmentId { get; set; }
        public int? TaskType { get; set; }
        public bool IsRepair { get; set; }
        public List<MBOutGateWayStatusItem> OutStatusList { get; set; }
    }
    public class GetStockTaskEntityInput
    {
        public string? CarTypeNum { get; set; }
        public int? AreaId { get; set; }
        public int? Status { get; set; }
        public int? EquipmentId { get; set; }
        public int? TaskType { get; set; }
        public bool? IsRepair { get; set; }

        public List<bool>  OutStatusList { get; set; }
        public int? GatewayId { get; set; }

    }
    public class GetStockTaskHistoryEntityInput
    {
        public string? CarTypeNum { get; set; }
        public int? AreaId { get; set; }
        public int? Status { get; set; }
        public int? EquipmentId { get; set; }
        public int? TaskType { get; set; }

        public int? GatewayId { get; set; }

    }


    public class GetRequestStockTaskEntityInput
    {
        public string CarTypeNum { get; set; }
        public int? AreaId { get; set; }
        public int? Status { get; set; }
        public int? EquipmentId { get; set; }
        public int? TaskType { get; set; }
        public List<int> OutGateWayIds { get; set; }
    }

    public class AddInStockTaskInput
    {
        public int TaskType { get; set; }
        /// <summary>
        /// 台车编号
        /// </summary>
        public string CarTypeNum { get; set; }
        public int? GatewayId { get; set; }
        public int? LocationId { get; set; }

        public int? CarTypeId { get; set; }

        public string Remark { get; set; }
        public bool IsRepair { get; set; }

        public string RFIDTen { get; set; } = "";//夹具库新增十进制RFID
    }
    public class AddOutStockTaskInput
    {
        public List<int> InventoryIds { get; set; }
        public bool IsRepair { get; set; }
    }

    public class AddOutStockTaskToMesInput
    {
        /// <summary>
        /// 台车编号
        /// </summary>
        public string CarTypeNum { get; set; }
        /// <summary>
        /// 车型编号
        /// </summary>
        public string? CarTypeCode { get; set; }
        /// <summary>
        /// 操作区域 1主线 2是
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

    }

    public class AddRemoveStockTaskInput
    { 
        public int RemoveCount { get; set; }
        public List<int> InventoryIds { get; set; }
    }
    public class UpdatePriorityInput
    {
        public List<int> SelectIds { get; set; }
        public int Priority { get; set; }
    }
    public class UpdateTaskStatusInput
    {
        public List<int> SelectIds { get; set; }
        public int TaskStatus { get; set; }
    }

    public class SetSequenceInput
    {
        public int Id { get; set; }
        /// <summary>
        /// 是否上移
        /// </summary>
        public bool IsUp { get; set; }
    }

    public class AllocateLocationOutput
    {
        public List<Location> LocationList { get; set; }

    }

    public class JudgeLocationOutput
    {
        public Task taskEntity { get; set; }
        public Location locationEntity { get; set; }

        public Location qian_locationEntity { get; set; }
    }

    public class StockTaskCache
    {
        public TaskType LastTaskType { get; set; }

    }

    public class FinishedTaskAndAddMoveTaskInput
    {
        public int TaskId { get; set; }
    }

    public class GetTongJiTaskHistroyOutput
    {
        /// <summary>
        /// 今日入库数量
        /// </summary>
        public int TodayInStockCount { get; set; }
        /// <summary>
        /// 今日出库数量
        /// </summary>
        public int TodayOutStockCount { get; set; }
        /// <summary>
        /// 今年每月入库数量统计
        /// </summary>
        public int[] YearInStockCount { get; set; }
        /// <summary>
        /// 今年每月出库数量统计
        /// </summary>
        public int[] YearOutStockCount { get; set; }
    }

    #region 统计当日出库数据总和

    public class GetTongJiDayCount
    {
        /// <summary>
        /// 今日入库数量
        /// </summary>
        public int TodayInStockCount { get; set; }
        /// <summary>
        /// 今日出库数量
        /// </summary>
        public int TodayOutStockCount { get; set; }


        #region 统计堆垛机的出入库数量,只有主线库需要三台堆垛机，其他库默认使用第一台堆垛机的数量

        /// <summary>
        /// 一号机今日入库数量
        /// </summary>
        public int? E1TodayInStockCount { get; set; } = 0;

        /// <summary>
        /// 一号机今日出库数量
        /// </summary>
        public int? E1TodayOutStockCount { get; set; } = 0;

        /// <summary>
        /// 一号机本周入库数量
        /// </summary>
        public int? E1WeekInStockCount { get; set; } = 0;

        /// <summary>
        /// 一号机本周出库数量
        /// </summary>
        public int? E1WeekOutStockCount { get; set; } = 0;

        /// <summary>
        /// 一号机本月入库数量
        /// </summary>
        public int? E1MonthInStockCount { get; set; } = 0;

        /// <summary>
        /// 一号机本月出库数量
        /// </summary>
        public int? E1MonthOutStockCount { get; set; } = 0;


        /// <summary>
        /// 二号机今日入库数量
        /// </summary>
        public int? E2TodayInStockCount { get; set; } = 0;

        /// <summary>
        /// 二号机今日出库数量
        /// </summary>
        public int? E2TodayOutStockCount { get; set; } = 0;

        /// <summary>
        /// 二号机本周入库数量
        /// </summary>
        public int? E2WeekInStockCount { get; set; } = 0;

        /// <summary>
        /// 二号机本周出库数量
        /// </summary>
        public int? E2WeekOutStockCount { get; set; } = 0;

        /// <summary>
        /// 二号机本月入库数量
        /// </summary>
        public int? E2MonthInStockCount { get; set; } = 0;

        /// <summary>
        /// 二号机本月出库数量
        /// </summary>
        public int? E2MonthOutStockCount { get; set; } = 0;

        /// <summary>
        /// 三号机今日入库数量
        /// </summary>
        public int? E3TodayInStockCount { get; set; } = 0;

        /// <summary>
        /// 三号机今日出库数量
        /// </summary>
        public int? E3TodayOutStockCount { get; set; } = 0;

        /// <summary>
        /// 三号机本周入库数量
        /// </summary>
        public int? E3WeekInStockCount { get; set; } = 0;

        /// <summary>
        /// 三号机本周出库数量
        /// </summary>
        public int? E3WeekOutStockCount { get; set; } = 0;

        /// <summary>
        /// 三号机本月入库数量
        /// </summary>
        public int? E3MonthInStockCount { get; set; } = 0;

        /// <summary>
        /// 三号机本月出库数量
        /// </summary>
        public int? E3MonthOutStockCount { get; set; } = 0;

        #endregion
       
        /// <summary>
        /// 所有历史数据中入库集合
        /// </summary>
        public List<HistoryCartype> HistoryCartypesIn { get; set; } = new List<HistoryCartype>() { };
        /// <summary>
        /// 所有历史数据中出库集合
        /// </summary>
        public List<HistoryCartype> HistoryCartypesOut { get; set; } = new List<HistoryCartype>() { };

    }

    public class HistoryCartype
    {
        public string CarName { get; set; }//车型

        public int CountNumber { get; set; }//数量
    }
    #endregion
}