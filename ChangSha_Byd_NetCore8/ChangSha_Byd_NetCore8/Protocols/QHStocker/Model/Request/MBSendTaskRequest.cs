using ChangSha_Byd_NetCore8.Protocols.Common;
using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request
{
    public class MBSendTaskRequest : IRequest<MBSendTaskResponse>
    {
        public Common.TaskStatus TaskStatus { get; set; }
        public int? AreaId { get; set; }
        public int? TaskType { get; set; }
        public int? EquipmentId { get; set; }
        public bool IsRepair { get; set; } = false;

        /// <summary>
        /// 允许出库的信号
        /// </summary>
        public List<MBOutGateWayStatusItem> OutGateWayStatusList { get; set; }
    }


    public class MBOutGateWayStatusItem
    {
        public 主库_OutLocation outLocation { get; set; }

        public bool OutStatus { get; set; }
    }


    public class MBSendTaskResponse
    {
        /// <summary>
        /// 任务号
        /// </summary>
        public int TaskCode { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public PLCTaskType TaskType { get; set; }

        /// <summary>
        /// 源排号
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// 源层号
        /// </summary>
        public int StartFloor { get; set; }

        /// <summary>
        /// 源列号
        /// </summary>
        public int StartColumn { get; set; }

        /// <summary>
        /// 目的排号
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// 目的层号
        /// </summary>
        public int EndFloor { get; set; }

        /// <summary>
        /// 目的列号
        /// </summary>
        public int EndColumn { get; set; }


        /// <summary>
        /// 任务校验号
        /// </summary>
        public int VerificationCode { get; set; }

        /// <summary>
        /// 下发任务RIFD
        /// </summary>
        public string TaskRFID { get; set; }
    }

    /// <summary>
    /// 应该没有用到
    /// </summary>
    public enum 主库_EntryLocation
    {
        UB立库1巷道入口 = 1,
        UB立库2巷道入口 = 2,
        UB立库3巷道入口 = 3,
        主线立库1巷道入口 = 4,
        主线立库2巷道入口 = 5,
        主线立库3巷道入口 = 6
    }

    public enum 主库_OutLocation
    {
        UB立库1巷道出口 = 7,
        UB立库2巷道出口 = 8,
        UB立库3巷道出口 = 9,
        主线立库1巷道出口 = 10,
        主线立库2巷道出口 = 11,
        主线立库3巷道出口 = 12
    }
}
