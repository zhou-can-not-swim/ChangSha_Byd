namespace ChangSha_Byd_NetCore8.Extends.PlcServices
{
    public static class S7PlcOptions
    {
        public static IServiceCollection AddS7PlcOptions(this IServiceCollection services, IConfiguration plcsConfig)
        {
            ///把配置中的plcConnections节点下的所有子节点(127.0.0.1 , 0 , 1)都添加到服务中
            foreach (IConfigurationSection child in plcsConfig.GetChildren())
            {//child中sections中放了所有的配置绑定到S7PlcOptItem去
                services.AddOptions<S7PlcOptItem>(child.Key).Bind(child);
            }

            return services;
        }

        //todo 这个是不是没有使用到
        public static IServiceCollection AddS7PlcOptions(this IServiceCollection services, Action<S7PlcOptsBuilder> configure)
        {
            ///S7PlcOptsBuilder是用来构建PLC配置的
            S7PlcOptsBuilder s7PlcOptsBuilder = new S7PlcOptsBuilder();
            configure?.Invoke(s7PlcOptsBuilder);
            foreach (KeyValuePair<string, S7PlcOptItem> mapping in s7PlcOptsBuilder.BuildMaps())
            {
                services.AddOptions<S7PlcOptItem>(mapping.Key).Configure(delegate (S7PlcOptItem item)
                {
                    item.IpAddr = mapping.Value.IpAddr;
                    item.Rack = mapping.Value.Rack;
                    item.Slot = mapping.Value.Slot;
                });
            }

            return services;
        }
    }
}
