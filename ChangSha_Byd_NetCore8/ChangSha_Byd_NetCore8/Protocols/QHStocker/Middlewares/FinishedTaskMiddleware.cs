using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.fan.middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Request;
using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares
{
    /// <summary>
    /// 完成任务
    /// </summary>
    public class FinishedTaskMiddleware : IWorkMiddleware<ScanContext>
    {
        private readonly ILogger<FinishedTaskMiddleware> _logger;
        private readonly IMediator _mediator;

        public FinishedTaskMiddleware(ILogger<FinishedTaskMiddleware> logger, IMediator mediator)
        {
            this._logger = logger;
            this._mediator = mediator;
        }

        public async Task InvokeAsync(ScanContext context, WorkDelegate<ScanContext> next)
        {
            try
            {

                // 完成任务确认 置 1
                //13 0.3
                if (context.PlcInfo.FinishTaskReq &&
                    !context.MstMsg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.完成任务确认))
                {
                    #region  处理业务逻辑
                    PlcFinishedTaskRequest request = new PlcFinishedTaskRequest();
                    request.TaskNo = context.PlcInfo.TaskNo;
                    request.TaskStatus = (TaskStatus)Common.TaskStatus.任务完成;
                    var response = await _mediator.Send(request);

                    if (response.Result)
                    {
                        //var alarm = new AlarmMessage()
                        //{
                        //    EventSource = PlcNames.PlcName_QHStocker,
                        //    Content = $"任务编号：{request.TaskNo}完成",
                        //    Level = LogLevel.Information,
                        //    Timestamp = DateTime.Now,
                        //};
                        //var logmsg = new UILogNotificatinon(alarm);
                        //await this._mediator.Publish(logmsg);

                        MstFlagsGeneralBuilder builder = new MstFlagsGeneralBuilder(context.Pending.GeneralCmdWord);
                        context.Pending.GeneralCmdWord = builder.完成任务确认(true).Build();

                    }
                    else
                    {
                        ////业务处理失败
                        //var alarm = new AlarmMessage()
                        //{
                        //    EventSource = PlcNames.PlcName_QHStocker,
                        //    Content = $"任务编号：{request.TaskNo}处理完成服务失败",
                        //    Level = LogLevel.Error,
                        //    Timestamp = DateTime.Now,
                        //};
                        //var logmsg = new UILogNotificatinon(alarm);
                        //await this._mediator.Publish(logmsg);
                    }
                    #endregion



                }

                //完成任务确认 置 0
                if (!context.PlcInfo.FinishTaskReq && context.MstMsg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.完成任务确认))
                {
                    MstFlagsGeneralBuilder builder = new MstFlagsGeneralBuilder(context.Pending.GeneralCmdWord);
                    context.Pending.GeneralCmdWord = builder.完成任务确认(false).Build();
                }

            }
            finally
            {
                await next(context);
            }
        }
    }
}
