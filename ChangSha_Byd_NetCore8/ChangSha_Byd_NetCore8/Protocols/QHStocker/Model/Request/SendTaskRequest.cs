using ChangSha_Byd_NetCore8.Protocols.Common;
using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request
{
    public class SendTaskRequest : IRequest<SendTaskResponse>
    {
        public Common.TaskStatus TaskStatus { get; set; }
        public int? AreaId { get; set; }
        public TaskType TaskType { get; set; }
        public int? EquipmentId { get; set; }
        public bool IsRepair { get; set; } = false;
        /// <summary>
        /// 允许出库的信号
        /// </summary>
        public List<OutGateWayStatusItem> OutGateWayStatusList { get; set; }
    }


    public class OutGateWayStatusItem
    {
        //工位出口AB出口
        public QH_OutLocation outLocation { get; set; }

        public bool OutStatus { get; set; }
    }


    public class SendTaskResponse
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
}
