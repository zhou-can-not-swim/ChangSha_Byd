using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AsZero.DbContexts
{

    public class AsZeroDbContext : DbContext
    {
        private readonly IHostEnvironment _env;

        public AsZeroDbContext(DbContextOptions<AsZeroDbContext> options, IHostEnvironment env) : base(options)
        {
            this._env = env;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            SeedDatabase(modelBuilder);
            if (_env.IsDevelopment())
            {
                SeedDatabaseForDevelopment(modelBuilder);
            }
        }


        #region Seed Database
        private static void SeedDatabase(ModelBuilder modelBuilder)
        {

            //SeedWarehouse(modelBuilder);
           
        }

        private static void SeedWarehouse(ModelBuilder modelBuilder)
        {
            //var warehouseEntity = modelBuilder.Entity<State>();
            //var warehouse = new State() { Id = 1, EquipmentId = 1, EquipmentName = "主线堆垛机", WarehouseId = 1, WarehouseName = "主线库", Dname = "联机", Dtrip = "待机", TaskTime = DateTime.Now, Conent = "hhhh" };
            //warehouseEntity.HasData(warehouse);
        }

        private static void SeedDatabaseForDevelopment(ModelBuilder modelBuilder)
        {

           
        }
        #endregion

    


        #region wms
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<CarType> CarTypes { get; set; }        
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Area_CarType_GateWay> Area_CarType_GateWays { get; set; }
        public DbSet<Inventory> Inventorys { get; set; }

        public DbSet<StockTask> Tasks { get; set; }
        public DbSet<StockTaskHistory> TaskHistorys { get; set; }
        public DbSet<TempCode> TempCodes { get; set; }

        public DbSet<ProductionLog> ProductionLogs { get; set; }
        public DbSet<State> States { get; set; }
        #endregion

    }
}

