using ChangSha_Byd_NetCore8.Extends;
using ChangSha_Byd_NetCore8.Extends.Scan;
using MediatR;
using Microsoft.Extensions.Options;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker
{
    /// <summary>
    /// PlcHostedService 继承自 BackgroundService，它是一个长时间运行的后台服务
    /// ExecuteAsync 方法启动后，服务会一直运行直到应用程序关闭
    /// </summary>
    public class PlcHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _ssf;
        private readonly IMediator _mediator;
        private readonly ILogger<PlcHostedService> _logger;
        private readonly PlcMgr _plcmgr;
        private readonly IOptionsMonitor<ScanOpts> _scanOptionsMonitor;

        public PlcHostedService(IServiceScopeFactory ssf,
                                IMediator mediator,
                                ILogger<PlcHostedService> logger,
                                PlcMgr plcmgr,
                                IOptionsMonitor<ScanOpts> scanOpts)
        {
            _ssf = ssf;
            _mediator = mediator;
            _logger = logger;
            _plcmgr = plcmgr;
            _scanOptionsMonitor = scanOpts;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var thread = new Thread(() =>
            {
                _ = RunAsync(stoppingToken);
            });
            thread.Start();
            return Task.CompletedTask;
        }

        private async Task RunAsync(CancellationToken ct)
        {
            var DiscountConnectedCount = 0;
            var PlcStatus = "";
            while (!ct.IsCancellationRequested)
            {
                var scanOpt = _scanOptionsMonitor.CurrentValue;
                if (!scanOpt.QH_堆垛机.Enabled) //检查是否连接
                {
                    await Task.Delay(800, ct);
                    continue;
                }
                try
                {
                    //确保plc已连接，如果未连接会抛异常，进行下一次尝试连接
                    await _plcmgr.PlcName_QHStocker.EnsureConnectedAsync();
                    DiscountConnectedCount = 0;

                    //读plc给上位机部分
                    var plcOp = await _plcmgr.PlcName_QHStocker.ReadPlcMsgAsync();//13

                    //读上位机写的部分
                    var mstOp = await _plcmgr.PlcName_QHStocker.ReadMstMsgAsync();//11

                    if (plcOp.IsError)
                    {
                        throw new Exception(plcOp.ErrorValue);
                    }
                    if (mstOp.IsError)
                    {
                        throw new Exception(plcOp.ErrorValue);
                    }
                    var dt = DateTime.Now;

                    var plcmsg = plcOp.ResultValue;//plc信息
                    var mstmsg = mstOp.ResultValue;//msg信息

                    using var scope = _ssf.CreateScope();
                    var serviceprovider = scope.ServiceProvider;
                    //获取处理器
                    var processor = serviceprovider.GetRequiredService<ScanProcessor>();
                    //构建上下文
                    var context = new ScanContext(serviceprovider, plcmsg, mstmsg, dt);

                    ////记录堆垛机变化状态
                    //var tempPlcStatus = PlcNames.PlcName_QHStocker + "状态:" + context.PlcInfo.StorkerStatus + ";  行程：" + context.PlcInfo.StockerTrip + ";  是否载货：" + context.PlcInfo.StockerCargo + ";  动作：" + context.PlcInfo.StockerAction + ";";
                    //if (PlcStatus != tempPlcStatus)
                    //{
                    //    //记录到日志文件
                    //    var logger = StockerLogger.GetCustomLogger(PlcNames.PlcName_QHStocker, "前后地板库");
                    //    logger.Info(tempPlcStatus);

                    //    PlcStatus = tempPlcStatus;

                    //}

                    //处理器处理上下文
                    await processor.HandleAsync(context);
                }
                catch (Exception ex)
                {
                    try
                    {
                        var msg = ex.Message;
                        //1s一循环，循环了5次还是堆垛机连接失败，则报警
                        if (ex.Message.Contains("Connection Error"))
                        {
                            DiscountConnectedCount++;
                            //if (DiscountConnectedCount >= 5)
                            //{
                            //    var alarm = new AlarmMessage
                            //    {
                            //        EventSource = PlcNames.PlcName_QHStocker,
                            //        Content = "堆垛机连接失败",
                            //        EventNumber = "",
                            //        Level = LogLevel.Error,
                            //        Timestamp = DateTime.Now,
                            //    };
                            //    var logmsg = new UILogNotificatinon(alarm);
                            //    await _mediator.Publish(logmsg, ct);
                            //}

                        }
                        else
                        {
                            //var alarm = new AlarmMessage
                            //{
                            //    EventSource = PlcNames.PlcName_QHStocker,
                            //    Content = msg,
                            //    EventNumber = "",
                            //    Level = LogLevel.Error,
                            //    Timestamp = DateTime.Now,
                            //};
                            //var logmsg = new UILogNotificatinon(alarm);
                            //await _mediator.Publish(logmsg, ct);
                        }

                        await _plcmgr.PlcName_QHStocker.DisconnectAsync();
                    }
                    catch (Exception exc)
                    {
                        _logger.LogInformation($"尝试断开和[{PlcNames.PlcName_QHStocker}]连接失败: {exc.Message}");
                    }
                }
                finally
                {
                    _logger.LogDebug($"..和[{PlcNames.PlcName_QHStocker}]交互动作完成...");
                    await Task.Delay(500, ct);
                }
            }

            _logger.LogError($"和PLC[{PlcNames.PlcName_QHStocker}]交互服务结束");
        }

    }
}
