using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request
{
    public class RequestTaskRequest : IRequest<RequestTaskResponse>
    {
        public QH_OutLocation outLocation { get; set; }

        public string? RFid { get; set; }

        public bool? IsUsed { get; set; }
    }

    public class RequestTaskResponse
    {
        /// <summary>
        /// 下发任务库口号
        /// </summary>
        public QH_OutLocation outLocation { get; set; }
        /// <summary>
        /// 下发任务RIFD
        /// </summary>
        public string TaskRFID { get; set; }
    }

}
