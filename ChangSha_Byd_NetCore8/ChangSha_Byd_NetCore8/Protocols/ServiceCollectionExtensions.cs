using ChangSha_Byd_NetCore8.Extends;
using ChangSha_Byd_NetCore8.Extends.PlcServices;
using ChangSha_Byd_NetCore8.Extends.Scan;
using ChangSha_Byd_NetCore8.Protocols.QHStocker;

namespace ChangSha_Byd_NetCore8.Protocols
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Plc相关服务,为了在programe.cs中调用
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddPlcServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddS7PlcOptions(config);
            services.AddSingleton<PlcMgr>();
            services.AddPlcServicesForLine5_QHStocker();//添加QH相关服务,就在本文件中定义
            //services.AddPlcServicesForLine5_CWStocker();
            return services;
        }
        public static IServiceCollection AddScanOpts(this IServiceCollection services, IConfiguration canOptsConfig)
        {
            //添加扫描配置项，就是堆垛机，对enable进行确认，
            //如果这个时候堆垛机没有启动，那么就是fasle，这个时候就扫描不了middlewares
            services.Configure<ScanOpts>(canOptsConfig);
            return services;
        }

    }
}
