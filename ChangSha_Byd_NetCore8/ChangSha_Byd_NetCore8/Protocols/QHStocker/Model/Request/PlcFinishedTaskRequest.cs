using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request
{
    public class PlcFinishedTaskRequest : IRequest<PlcFinishedTaskResponse>
    {
        /// <summary>
        /// 任务号
        /// </summary>
        public int TaskNo { get; set; }
        /// <summary>
        /// 任务状态 初始化 = 0, 等待执行 = 1,  正在执行 = 2,任务完成 = 3, 故障 = 4,暂停 = 5,  取消任务 = 9
        /// </summary>
        public TaskStatus TaskStatus { get; set; }
    }

    public class PlcFinishedTaskResponse
    {
        public bool Result { get; set; }

    }
}
