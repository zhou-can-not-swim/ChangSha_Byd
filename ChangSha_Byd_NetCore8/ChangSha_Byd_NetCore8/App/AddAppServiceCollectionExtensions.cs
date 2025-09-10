using ChangSha_Byd_NetCore8.App.Production;
using ChangSha_Byd_NetCore8.App.WarehouseModel;

namespace ChangSha_Byd_NetCore8.App
{
    public static class AddAppServiceCollectionExtensions
    {
        public static IServiceCollection AddApps(this IServiceCollection services)
        {

            services.AddScoped<AreaApp>();
            services.AddScoped<EquipmentApp>();
            services.AddScoped<CarTypeApp>();
            services.AddScoped<GatewayApp>();
            services.AddScoped<LocationApp>();
            services.AddScoped<WarehouseApp>();
            services.AddScoped<Area_CarType_GateWayApp>();
            services.AddScoped<TempCodeApp>();
            services.AddScoped<InventoryApp>();
            services.AddScoped<StockTaskApp>();
            services.AddScoped<StockTaskHistoryApp>();
            services.AddScoped<StateApp>();
            return services;
        }
    }
}
