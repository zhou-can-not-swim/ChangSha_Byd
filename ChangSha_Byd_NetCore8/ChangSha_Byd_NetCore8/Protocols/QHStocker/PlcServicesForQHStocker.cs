using ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Middlewares.PublishNotification;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker
{
    public static class PlcServicesForQHStocker
    {
        /// <summary>
        /// PLC相关服务详情配置 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddPlcServicesForLine5_QHStocker(this IServiceCollection services)
        {
            // background services & plc processor
            services.AddHostedService<PlcHostedService>();
            services.AddSingleton<ScanProcessor>();

            #region 中间件
            services.AddScoped<PublishNotificationMiddleware>();
            services.AddScoped<EntryArrivedMiddleware>();
            //services.AddScoped<RequestTaskMiddleware>();
            services.AddScoped<RequestOutTaskMiddleware>();
            services.AddScoped<SendTaskMiddleware>();
            services.AddScoped<FinishedTaskMiddleware>();
            services.AddScoped<HeartBeatMiddleware>();
            services.AddScoped<ReadAndWritePlcMiddleware>();//前端读写plc
            services.AddScoped<FlushPendingMiddleware>();//写入plc
            #endregion

            return services;
        }
    }
}
