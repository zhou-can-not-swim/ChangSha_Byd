using AsZero.DbContexts;
using Byd.Services.Request;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.App.WarehouseModel;
using ChangSha_Byd_NetCore8.Entities;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.Hub;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using ChangSha_Byd_NetCore8.Protocols.Common;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Infra;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TaskStatus = ChangSha_Byd_NetCore8.Protocols.Common.TaskStatus;

namespace ChangSha_Byd_NetCore8.App.Production
{   
    public class StockTaskApp : FutureBaseEntityService<int, StockTask>
    {
        private readonly IOptions<AppSetting> _appConfiguration;
        public readonly AsZeroDbContext _dBContext;
        private readonly IHubContext<ProductionHub, IProductionHub> _hubContext;

        public readonly LocationApp _locationApp;
        public readonly InventoryApp _inventoryApp;
        private readonly IMemoryCache _memoryCache;
        private readonly TempCodeApp _tempCodeApp;
        private readonly Area_CarType_GateWayApp _area_CarType_GateWayApp;
        private readonly CarTypeApp _carTypeApp;
        private readonly GatewayApp _gatewayApp;
        private readonly StockTaskHistoryApp _stockTaskHistoryApp;

        public StockTaskApp(IGenericRepository<int, StockTask> repo, AsZeroDbContext dBContext, LocationApp locationApp, InventoryApp stockRecordApp, TempCodeApp tempCodeApp,
            Area_CarType_GateWayApp area_CarType_GateWayApp, CarTypeApp carTypeApp, GatewayApp gatewayApp, StockTaskHistoryApp stockTaskHistoryApp, IMemoryCache memoryCache,
            IOptions<AppSetting> appConfiguration, IHubContext<ProductionHub, IProductionHub> hubContext) : base(repo)
        {
            _dBContext = dBContext;
            _locationApp = locationApp;
            _inventoryApp = stockRecordApp;
            _tempCodeApp = tempCodeApp;
            _area_CarType_GateWayApp = area_CarType_GateWayApp;
            _carTypeApp = carTypeApp;
            _gatewayApp = gatewayApp;
            _stockTaskHistoryApp = stockTaskHistoryApp;
            _memoryCache = memoryCache;
            _appConfiguration = appConfiguration;
            _hubContext = hubContext;
        }

        public async Task<TableData> Load(QueryStockTaskReq request)
        {
            var query = new Specification<StockTask>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.CarTypeNum.Contains(request.key));
            }

            if (request.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == request.AreaId);
            }

            if (request.Type != null)
            {
                query.CombineCritia(u => (int)u.TaskType == request.Type);
            }

            if (request.Status != null)
            {
                query.CombineCritia(u => (int)u.TaskStatus == request.Status);
            }

            if (request.NotStatus != null)
            {
                query.CombineCritia(u => (int)u.TaskStatus != request.NotStatus);
            }

            var pageSpec = query.New().ApplyOrderByDescending(a => a.Priority).ApplyOrderBy(a => a.Id).ApplyPaging(new Pagination(request.page, request.limit));

            var (count, data) = await LoadPageAsNoTrackingAsync(query, pageSpec);
            return new TableData { count = count, data = data };
        }

        public async Task<IReadOnlyList<StockTask>> GetList(GetStockTaskListInput request)
        {
            var query = new Specification<StockTask>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.CarTypeNum.Contains(request.key));
            }

            if (request.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == request.AreaId);
            }

            if (request.Type != null)
            {
                query.CombineCritia(u => (int)u.TaskType == request.Type);
            }

            if (request.Status != null)
            {
                query.CombineCritia(u => (int)u.TaskStatus == request.Status);
            }

            if (request.NotStatus != null)
            {
                query.CombineCritia(u => (int)u.TaskStatus != request.NotStatus);
            }

            if (request.EquipmentId != null)
            {
                query.CombineCritia(u => u.EquipmentId == request.EquipmentId);
            }

            var list = await Repository.Query(query.New().ApplyOrderByDescending(a => a.Priority).ApplyOrderBy(a => a.Id)).AsNoTracking().ToListAsync();


            return list;
        }

        public async Task<bool> IsHaveCode(string code)
        {
            var query = new Specification<StockTask>(a => !a.IsDeleted && a.Code == code);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var i = await Repository.Query(query).AsNoTracking().CountAsync();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取StockTask对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<StockTask> GetStockTaskEntity(GetStockTaskEntityInput input)
        {
            var query = new Specification<StockTask>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);//查询操作
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }

            if (input.Status != null)
            {
                query.CombineCritia(u => (int)u.TaskStatus == input.Status);
            }

            if (input.EquipmentId != null)
            {
                query.CombineCritia(u => u.EquipmentId == input.EquipmentId);
            }

            if (input.TaskType != null)
            {
                query.CombineCritia(u => u.TaskType == (TaskType)input.TaskType);
            }

            if (input.GatewayId != null)
            {
                query.CombineCritia(u => u.GatewayId == input.GatewayId);
            }

            if (input.IsRepair != null)
            {
                query.CombineCritia(u => u.IsRepair == input.IsRepair);
            }

            if (input.CarTypeNum != null)
            {
                query.CombineCritia(u => u.CarTypeNum == input.CarTypeNum);
            }

            if (input.TaskType == 2)//任务号为2是出库
            {
                //if (currentWarehouseId == 6)
                //{
                //    //代表侧围库
                //    var s = await this.Repository.Query(query.New().ApplyOrderBy(a => a.GatewayId).ApplyOrderBy(a => a.CarTypeNum)).AsNoTracking().FirstOrDefaultAsync();

                //    return s;
                //}
                //else
                //{
                //    //如果是出库任务，要先出B面，再出A面
                //    return await this.Repository.Query(query.New().ApplyOrderByDescending(a => a.CarTypeFace).ApplyOrderByDescending(a => a.Priority).ApplyOrderBy(a => a.Id))
                //        .AsNoTracking().FirstOrDefaultAsync();
                //}

                //按照库口顺序出库
                return await Repository.Query(
                    query.New()
                    .ApplyOrderBy(a => a.GatewayId)
                    .ApplyOrderByDescending(a => a.Priority)
                    .ApplyOrderBy(a => a.Id))
                    .AsNoTracking()
                    .FirstOrDefaultAsync();





            }
            else
            {
                //入库和移库在这里操作
                return await Repository.Query(
                    query.New()
                    .ApplyOrderByDescending(a => a.Priority)
                    .ApplyOrderBy(a => a.Id))
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
        }


        /// <summary>
        /// 获取请求出库的StockTask对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<StockTask> GetRequstStockTaskEntity(GetRequestStockTaskEntityInput input)
        {
            var query = new Specification<StockTask>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }

            if (!string.IsNullOrEmpty(input.CarTypeNum))
            {
                query.CombineCritia(u => u.CarTypeNum == input.CarTypeNum);
            }

            if (input.Status != null)
            {
                query.CombineCritia(u => (int)u.TaskStatus == input.Status);
            }

            if (input.EquipmentId != null)
            {
                query.CombineCritia(u => u.EquipmentId == input.EquipmentId);
            }

            if (input.TaskType != null)
            {
                query.CombineCritia(u => u.TaskType == (TaskType)input.TaskType);
            }

            if (input.OutGateWayIds != null && input.OutGateWayIds.Count > 0)
            {
                query.CombineCritia(u => input.OutGateWayIds.Contains((int)u.GatewayId));
            }

            if (input.TaskType == 2)//出库任务
            {
                //如果是出库任务，要先出B面，再出A面
                return await Repository.Query(
                    query.New()
                    .ApplyOrderByDescending(a => a.CarTypeFace) //ab面
                    .ApplyOrderByDescending(a => a.Priority)    //优先级
                    .ApplyOrderBy(a => a.RequestTaskTime))     //分拼请求校验RFID时间
                    .AsNoTracking().FirstOrDefaultAsync();
            }
            else
            {
                //这个应该是入库任务
                return await Repository.Query(
                    query.New().ApplyOrderByDescending(a => a.Priority)
                    .ApplyOrderBy(a => a.RequestTaskTime))
                    .AsNoTracking().FirstOrDefaultAsync();
            }
        }



        //UNDONE :根本就没有这个 手动入库 的操作 
        ///// <summary>
        ///// 用法1：添加手动入库任务
        ///// 用法2：入口来料plc给信号请求入库
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<StockTask> AddInStockTask(
        //    AddInStockTaskInput input, 
        //    CarType carType, 
        //    Area_CarType_GateWay area_CarType_GateWayEntity, 
        //    Location locationEntity,
        //    string fengcheType = ""
        //)
        //{
        //    var currentWarehouseId = _appConfiguration.Value.WarehouseId;
        //    using (var transaction = _dBContext.Database.BeginTransaction()) //开启事务
        //    {
        //        try
        //        {
        //            StockTask entity = new StockTask();
        //            entity.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //            entity.WarehouseId = currentWarehouseId;
        //            entity.AreaId = area_CarType_GateWayEntity.AreaId;
        //            entity.AreaName = area_CarType_GateWayEntity.Area.Name;
        //            entity.TaskType = TaskType.入库;
        //            entity.IsRepair = input.IsRepair;
        //            entity.Remark = input.Remark;
        //            entity.TaskStatus = Protocols.Common.TaskStatus.等待执行;
        //            entity.Priority = TaskPriority.普通;
        //            entity.CarTypeNum = input.CarTypeNum;
        //            entity.CarTypeId = carType.Id;


        //            //抓取当字符串中数字部分
        //            int result_shuzi = int.Parse(System.Text.RegularExpressions.Regex.Replace(input.CarTypeNum, @"[^0-9]+", ""));
        //            //抓取当字符串中字符部分
        //            string result_zifu = System.Text.RegularExpressions.Regex.Replace(input.CarTypeNum, @"\d", "");


        //            //if (!string.IsNullOrEmpty(result_zifu))//有字符的
        //            //{
        //            //    entity.CarTypeName = carType.Name;
        //            //    entity.CarTypeInt = int.Parse(result_shuzi.ToString().Substring(0, 1));
        //            //    if (result_shuzi >= 10000)
        //            //    {
        //            //        entity.CarTypeFace = 1;//主线
        //            //    }
        //            //    else
        //            //    {
        //            //        entity.CarTypeFace = 2;//UB
        //            //    }
        //            //}
        //            if (currentWarehouseId == 1) //侧围轮罩库  区域 车型 工位 ab面
        //            {

        //                var chexingnumber = input.CarTypeNum.Substring(1, 1); //第二位是车型
        //                var tempCategory = await _osContext.Categories.Where(a => !a.IsDeleted && a.TypeId == "CarType" && a.DtValue == chexingnumber).AsNoTracking()
        //                    .FirstOrDefaultAsync();

        //                if (tempCategory != null)
        //                {
        //                    //有车型的情况下
        //                    entity.CarTypeName = tempCategory.Name; //赋值车型名字
        //                    entity.CarTypeInt = tempCategory.DtValue; //车型
        //                }
        //                else
        //                {
        //                    //没有车型的情况下
        //                    entity.CarTypeName = "其他车型";
        //                    entity.CarTypeInt = "0"; //车型
        //                }

        //                if (input.CarTypeNum.Substring(3, 1) == "1")
        //                {
        //                    entity.CarTypeName += "_" + carType.Name + "_A面";
        //                    entity.CarTypeFace = 1;
        //                }
        //                else
        //                {
        //                    entity.CarTypeName += "_" + carType.Name + "_B面";
        //                    entity.CarTypeFace = 2;
        //                }

        //                //entity.CarTypeInt = tempCategory.DtValue; //车型
        //                entity.JXCarTypeNum = input.RFIDTen;//夹具库新增RFID十进制





        //                //var rdidQ = input.CarTypeNum.Substring(0, 2);
        //                //var rfidT = input.CarTypeNum.Substring(1, 1);
        //                //var rfidLast = input.CarTypeNum.Substring(2, input.CarTypeNum.Length - 2);

        //                ////十六进制转十进制
        //                //var rfidNumcode = Convert.ToInt32(rfidLast, 16);

        //                //var isrdidZifu = System.Text.RegularExpressions.Regex.Replace(rdidQ, @"\d", "");
        //                //if (!string.IsNullOrEmpty(isrdidZifu))
        //                //{
        //                //    //MB
        //                //    //判断是四线还是三线主线库
        //                //    if (lineId == 4)
        //                //    {
        //                //        entity.JXCarTypeNum = "AEC4" + rfidT + rfidNumcode.ToString().PadLeft(3, '0');
        //                //    }
        //                //    else
        //                //    {
        //                //        entity.JXCarTypeNum = "AEC3" + rfidT + rfidNumcode.ToString().PadLeft(3, '0');
        //                //    }

        //                //    entity.CarTypeFace = 1; //MB线体区域(此字段当作视图中线体区域部分,只有主线库有特殊区别)

        //                //}
        //                //else
        //                //{
        //                //    //UB
        //                //    //判断是四线还是三线主线库
        //                //    if (lineId==4)
        //                //    {
        //                //        entity.JXCarTypeNum = "AECU4" + rfidT + rfidNumcode.ToString().PadLeft(2, '0');
        //                //    }
        //                //    else
        //                //    {
        //                //        entity.JXCarTypeNum = "AECU3" + rfidT + rfidNumcode.ToString().PadLeft(2, '0');
        //                //    }

        //                //    entity.CarTypeFace = 2; //UB线体区域(此字段当作视图中线体区域部分,只有主线库有特殊区别)

        //                //}

        //                //entity.CarTypeName = carType.Name;
        //                ////UB区域
        //                //entity.CarTypeInt = fengcheType;
        //            }
        //            else //其他库
        //            {
        //                //其他夹具库为了区别3,4,5线八种夹具名字
        //                var chexingnumber = input.CarTypeNum.Substring(1, 1); //第二位是车型
        //                var tempCategory = await _osContext.Categories.Where(a => !a.IsDeleted && a.TypeId == "CarType" && a.DtValue == chexingnumber).AsNoTracking()
        //                    .FirstOrDefaultAsync();

        //                if (tempCategory != null)
        //                {
        //                    //有车型的情况下
        //                    entity.CarTypeName = tempCategory.Name; //赋值车型名字
        //                    entity.CarTypeInt = tempCategory.DtValue; //车型
        //                }
        //                else
        //                {
        //                    //没有车型的情况下
        //                    entity.CarTypeName = "其他车型";
        //                    entity.CarTypeInt = "0"; //车型
        //                }

        //                //从索引3开始取长度为1的字符 -->取第4个字符是不是1
        //                if (input.CarTypeNum.Substring(3, 1) == "1")
        //                {
        //                    entity.CarTypeName += "_" + carType.Name + "_A面";
        //                    entity.CarTypeFace = 1;
        //                }
        //                else
        //                {
        //                    entity.CarTypeName += "_" + carType.Name + "_B面";
        //                    entity.CarTypeFace = 2;
        //                }

        //                //entity.CarTypeInt = tempCategory.DtValue; //车型
        //                entity.JXCarTypeNum = input.RFIDTen;//夹具库新增RFID十进制
        //            }

        //            entity.CreateTime = DateTime.Now;

        //            entity.GatewayId = area_CarType_GateWayEntity.InGatewayId;
        //            entity.GatewayName = area_CarType_GateWayEntity.InGateway.Name;
        //            entity.SetTaskType = 0;

        //            entity.EquipmentId = locationEntity.EquipmentId;
        //            entity.EquipmentName = locationEntity.Equipment.Name;
        //            entity.TaskNo = await _tempCodeApp.GetNewCode();
        //            //取库位(新加的)
        //            entity.OutLocationId = area_CarType_GateWayEntity.InGatewayId;
        //            entity.OutLocationCode = area_CarType_GateWayEntity.InGateway.LocationCode;
        //            //放库位
        //            entity.InLocationId = locationEntity.Id;
        //            entity.InLocationCode = locationEntity.Code;
        //            entity.LocationType = locationEntity.LocationType;

        //            var res = await Repository.AddAsync(entity, false);

        //            //修改库位状态
        //            locationEntity.LocationStatus = LocationStatus.锁定;
        //            locationEntity.Area = null;
        //            locationEntity.Equipment = null;
        //            locationEntity.Warehouse = null;
        //            await _locationApp.UpdateForTrackedAsync(locationEntity, false);

        //            await Repository.SaveChangesAsync();
        //            transaction.Commit();       //提交事务
        //            return res;
        //        }
        //        catch (Exception ex)
        //        {
        //            string a = ex.Message;
        //            transaction.Rollback();     //回滚事务
        //            return null;
        //        }
        //    }
        //}


        /// <summary>
        /// 任务是否已存在
        /// </summary>
        /// <param name="CarTypeNum">16进制rfid</param>
        /// <param name="tt">task是出库还是入库</param>
        /// <returns></returns>
        public async Task<Response<bool>> IsHaveStockTask(string CarTypeNum, TaskType tt)
        {
            Response<bool> result = new Response<bool>();

            //在数据库中查找任务
            var query = new Specification<StockTask>(a => !a.IsDeleted);//找未删除
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);//当前仓库ID
            query.CombineCritia(u => u.CarTypeNum == CarTypeNum);//车型号
            query.CombineCritia(u => u.TaskType == tt);          //任务类型是出库还是入库
            
            var entity = await this.Repository.Query(query.New()).AsNoTracking().FirstOrDefaultAsync();
            if (entity != null)
            {
                result.Result = false;
                result.Message = "任务已存在!";
            }
            else
            {
                if (tt == TaskType.入库)
                {
                    //如果是入库处了查询任务队列有没有这个rfid，还要查询库存里有没有
                    GetInventoryListInput input = new GetInventoryListInput()
                    {
                        CarTypeNum = CarTypeNum
                    };
                    //开始查库存，防止这个输入的rfid在库存中存在，会造成重复
                    var i = await _inventoryApp.GetInventoryEntity(input);
                    if (i != null)
                    {
                        result.Result = false;
                        result.Message = "该台车已在立库!";
                    }
                    else
                    {
                        result.Result = true;
                    }
                }
                else
                {
                    result.Result = true;
                }
            }
            return result;
        }



        /// <summary>
        /// 前端添加手动出库任务
        /// 提供给MES的下出库任务也调用此方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> AddOutStockTask(IReadOnlyList<Inventory> inventoryList, bool isRepair, bool isShoudong)
        {
            using (var transaction = _dBContext.Database.BeginTransaction())
            {
                try
                {
                    GetArea_CarType_GateWayListInput getArea_CarType_GateWayListInput = new GetArea_CarType_GateWayListInput();
                    getArea_CarType_GateWayListInput.IsRepair = isRepair;
                    var area_CarType_GateWayList = await _area_CarType_GateWayApp.GetArea_CarType_GateWayList(getArea_CarType_GateWayListInput);

                    QueryLocationReq queryLocationReq = new QueryLocationReq();
                    var locationList = await _locationApp.GetList(queryLocationReq);

                    List<StockTask> addList = new List<StockTask>();
                    List<Location> updateList = new List<Location>();
                    List<Inventory> updateInventoryList = new List<Inventory>();
                    foreach (var item in inventoryList)
                    {
                        //检测任务是否已存在
                        var isHaveTask = await this.IsHaveStockTask(item.CarTypeNum, TaskType.出库);
                        if (isHaveTask.Result)
                        {
                            //判断库位状态是不是被禁用，如果禁用了则不出库
                            var locationEntity = locationList.Where(a => a.Id == item.LocationId).FirstOrDefault();
                            //获取仓库（四线主线库和三线和五线主线库以及四线其他库使用这个模板）
                            if (_appConfiguration.Value.WarehouseId == 1 || _appConfiguration.Value.LineId == 4 || _appConfiguration.Value.LineId == 3 ||
                                _appConfiguration.Value.LineId == 5)
                            {
                                if (!locationEntity.IsDisabled && !locationEntity.IsDeleted)
                                {
                                    StockTask entity = new StockTask();
                                    entity.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                    entity.WarehouseId = _appConfiguration.Value.WarehouseId;
                                    entity.AreaId = item.AreaId;
                                    entity.AreaName = item.AreaName;
                                    entity.TaskType = TaskType.出库;
                                    entity.IsRepair = isRepair;
                                    entity.TaskStatus = Protocols.Common.TaskStatus.已校验等待出库;
                                    entity.Priority = TaskPriority.普通;
                                    entity.CarTypeNum = item.CarTypeNum;
                                    entity.CarTypeFace = item.CarTypeFace;
                                    entity.CarTypeId = (int)item.CarTypeId;
                                    entity.CarTypeName = item.CarTypeName;
                                    entity.CarTypeInt = item.CarTypeInt;
                                    entity.CreateTime = DateTime.Now;
                                    //新增的
                                    entity.JXCarTypeNum = item.JXCarTypeNum;

                                    var area_CarType_GateWayEntity = area_CarType_GateWayList.Where(a => a.AreaId == item.AreaId && a.CarTypeId == item.CarTypeId).FirstOrDefault();

                                    entity.GatewayId = area_CarType_GateWayEntity.OutGatewayId;
                                    entity.GatewayName = area_CarType_GateWayEntity.OutGateway.Name;
                                    entity.SetTaskType = 0;


                                    entity.EquipmentId = locationEntity.EquipmentId;
                                    entity.EquipmentName = locationEntity.Equipment.Name;
                                    entity.TaskNo = await _tempCodeApp.GetNewCode();
                                    entity.OutLocationId = locationEntity.Id;
                                    entity.OutLocationCode = locationEntity.Code;
                                    entity.LocationType = locationEntity.LocationType;
                                    //出库的时候显示放货位置(新加的)
                                    entity.InLocationId = area_CarType_GateWayEntity.OutGatewayId;
                                    entity.InLocationCode = area_CarType_GateWayEntity.OutGateway.LocationCode;

                                    if (isRepair)
                                    {
                                        //维修出库不需要校验RFID
                                        entity.TaskStatus = Protocols.Common.TaskStatus.已校验等待出库;
                                        if (isShoudong)
                                        {
                                            entity.Remark = "手动下发维修出库任务。";
                                        }
                                        else
                                        {
                                            entity.Remark = "MES下发维修出库任务。";
                                        }
                                    }
                                    else
                                    {
                                        if (isShoudong)
                                        {
                                            entity.Remark = "手动下发出库任务。";
                                        }
                                        else
                                        {
                                            entity.Remark = "MES下发出库任务。";
                                        }
                                    }

                                    addList.Add(entity);

                                    //修改库位状态
                                    locationEntity.LocationStatus = LocationStatus.锁定;
                                    locationEntity.Area = null;
                                    locationEntity.Equipment = null;
                                    locationEntity.Warehouse = null;
                                    updateList.Add(locationEntity);

                                    //库存状态修改即将出库
                                    item.Status = 2;
                                    updateInventoryList.Add(item);
                                }
                            }
                            else
                            {
                                if (!locationEntity.IsDisabled && !locationEntity.IsDeleted)
                                {
                                    StockTask entity = new StockTask();
                                    entity.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                    entity.WarehouseId = _appConfiguration.Value.WarehouseId;
                                    entity.AreaId = item.AreaId;
                                    entity.AreaName = item.AreaName;
                                    entity.TaskType = TaskType.出库;
                                    entity.IsRepair = isRepair;
                                    entity.TaskStatus = Protocols.Common.TaskStatus.等待执行;
                                    entity.Priority = TaskPriority.普通;
                                    entity.CarTypeNum = item.CarTypeNum;
                                    entity.CarTypeFace = item.CarTypeFace;
                                    entity.CarTypeId = (int)item.CarTypeId;
                                    entity.CarTypeName = item.CarTypeName;
                                    entity.CarTypeInt = item.CarTypeInt;
                                    entity.CreateTime = DateTime.Now;
                                    //新增的
                                    entity.JXCarTypeNum = item.JXCarTypeNum;

                                    var area_CarType_GateWayEntity = area_CarType_GateWayList.Where(a => a.AreaId == item.AreaId && a.CarTypeId == item.CarTypeId).FirstOrDefault();

                                    entity.GatewayId = area_CarType_GateWayEntity.OutGatewayId;
                                    entity.GatewayName = area_CarType_GateWayEntity.OutGateway.Name;
                                    entity.SetTaskType = 0;


                                    entity.EquipmentId = locationEntity.EquipmentId;
                                    entity.EquipmentName = locationEntity.Equipment.Name;
                                    entity.TaskNo = await _tempCodeApp.GetNewCode();
                                    entity.OutLocationId = locationEntity.Id;
                                    entity.OutLocationCode = locationEntity.Code;
                                    entity.LocationType = locationEntity.LocationType;
                                    if (isRepair)
                                    {
                                        //维修出库不需要校验RFID
                                        entity.TaskStatus = TaskStatus.已校验等待出库;
                                        if (isShoudong)
                                        {
                                            entity.Remark = "手动下发维修出库任务。";
                                        }
                                        else
                                        {
                                            entity.Remark = "MES下发维修出库任务。";
                                        }
                                    }
                                    else
                                    {
                                        if (isShoudong)
                                        {
                                            entity.Remark = "手动下发出库任务。";
                                        }
                                        else
                                        {
                                            entity.Remark = "MES下发出库任务。";
                                        }
                                    }

                                    addList.Add(entity);

                                    //修改库位状态
                                    locationEntity.LocationStatus = LocationStatus.锁定;
                                    locationEntity.Area = null;
                                    locationEntity.Equipment = null;
                                    locationEntity.Warehouse = null;
                                    updateList.Add(locationEntity);

                                    //库存状态修改即将出库
                                    item.Status = 2;
                                    updateInventoryList.Add(item);
                                }
                            }
                        }
                    }

                    await Repository.AddRangeAsync(addList, false);
                    await _locationApp.UpdateRangeForTrackedAsync(updateList, false);
                    await _inventoryApp.UpdateRangeForTrackedAsync(updateInventoryList, false);
                    await Repository.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 添加出库任务 -> 提供给MES的接口 调用一次只生成一个出库任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Response<bool>> AddOutStockTaskToMes(AddOutStockTaskToMesInput input)
        {
            var result = new Response<bool>();
            try
            {
                if (input.Code != "byd123")
                {
                    result.Result = false;
                    result.Code = 500;
                    result.Message = "验证码错误";
                }
                else
                {
                    if (string.IsNullOrEmpty(input.CarTypeCode) && string.IsNullOrEmpty(input.CarTypeNum))
                    {
                        //都为空的情况下
                        result.Result = false;
                        result.Code = 500;
                        result.Message = "参数台车编号和车型编号不能同时为空";
                    }
                    // else if (input.CarTypeCode != 0 && !string.IsNullOrEmpty(input.CarTypeNum))
                    // {
                    //     //两个都填的情况下
                    //     result.Result = false;
                    //     result.Code = 500;
                    //     result.Message = "参数台车编号和车型编号有且只能有一个值";
                    // }
                    else
                    {
                        int WarehouseId = _appConfiguration.Value.WarehouseId;

                        if (string.IsNullOrEmpty(input.CarTypeCode))
                        {
                            //第一种情况

                            #region 按照类型出库

                            GetInventoryListInput getInventoryListInput = new GetInventoryListInput();
                            getInventoryListInput.CarTypeInt = input.CarTypeCode;
                            //if (input.AreaId == 1)//主线库
                            //{
                            //    getInventoryListInput.CarTypeFace = 1;
                            //}
                            //else if (input.AreaId == 2)//UB
                            //{
                            //    getInventoryListInput.CarTypeFace = 2;
                            //}
                            //else if (input.AreaId == 3)//其他
                            //{

                            //}
                            //getInventoryListInput.AreaId = input.AreaId;
                            getInventoryListInput.Status = 1; //正常库存
                            var inventoryEntity = await _inventoryApp.GetInventoryEntity(getInventoryListInput);
                            if (inventoryEntity != null)
                            {
                                List<Inventory> list = new List<Inventory>();
                                list.Add(inventoryEntity);
                                var b = await AddOutStockTask(list, false, false);
                                if (b)
                                {
                                    result.Result = true;
                                    result.Code = 200;
                                    result.Message = "添加出库任务成功";
                                }
                                else
                                {
                                    result.Result = false;
                                    result.Code = 500;
                                    result.Message = "添加出库任务失败";
                                }
                            }
                            else
                            {
                                result.Result = false;
                                result.Code = 500;
                                result.Message = "系统中未找到车型" + input.CarTypeCode + "库存";
                            }

                            #endregion
                        }
                        else if (!string.IsNullOrEmpty(input.CarTypeNum))
                        {
                            #region 按照台车编号出库 (只用RFID出库(CarTypeCode和AreadId==0))

                            GetInventoryListInput getInventoryListInput = new GetInventoryListInput()
                            {
                                CarTypeNum = input.CarTypeNum,
                                Status = 1
                            };
                            var inventoryEntity = await _inventoryApp.GetInventoryEntity(getInventoryListInput);
                            if (inventoryEntity != null)
                            {
                                List<Inventory> list = new List<Inventory>();
                                list.Add(inventoryEntity);
                                var b = await AddOutStockTask(list, false, false);
                                if (b)
                                {
                                    result.Result = true;
                                    result.Code = 200;
                                    result.Message = "添加出库任务成功";
                                }
                                else
                                {
                                    result.Result = false;
                                    result.Code = 500;
                                    result.Message = "添加出库任务失败";
                                }
                            }
                            else
                            {
                                result.Result = false;
                                result.Code = 500;
                                result.Message = "系统中未找到台车编号" + input.CarTypeNum + "库存";
                            }

                            #endregion
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Code = 500;
                result.Message = ex.Message;
                return result;
            }
        }

        //public async Task<Response<bool>> GetPlcStateToMes()
        //{
        //    var result = new Response<bool>();
        //    return result;
        //}

        #region 移库 更新任务级 任务状态  没使用过
        ///// <summary>
        ///// 前端添加手动移库任务，我好像没怎么用到移库
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<bool> AddRemoveStockTask(Inventory inventoryEntity, int RemoveCount)
        //{
        //    using (var transaction = _dBContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            //首先判断是哪一个库,主线库的话，分区域
        //            if (_appConfiguration.Value.WarehouseId == 1)
        //            {
        //                //获取同个区域的未禁用，未使用的库位
        //                QueryLocationReq queryLocationReq = new QueryLocationReq() { IsDisabled = false, AreaId = inventoryEntity.AreaId, Status = 0, IsKukou = false };
        //                var locationList = await _locationApp.GetList(queryLocationReq);
        //                var MaxCloumn = locationList.OrderByDescending(s => s.ColumnNo).Take(5).ToList(); //查询集合对象中某库位最大的列数按倒序排列，并且返回前五条数据
        //                var MaxFloor = MaxCloumn.OrderByDescending(s => s.FloorNo).Take(1).ToList(); //根据查询集合对象中某库位最大的列数再次根据最大层数排列，并且返回第一条数据(最终要到的列数)
        //                var MinCloumn = locationList.OrderBy(t => t.ColumnNo).Take(5).ToList(); //根据查询集合对象中某库位最小的列数排序，并且返回前五条数据
        //                var MinFloor = MinCloumn.OrderBy(t => t.FloorNo).Take(1).ToList(); //根据查询集合对象中某库位最小的列数排序再次根据最小层数排列，并且返回第一条数据(最终要得到的最小列数)
        //                List<StockTask> addList = new List<StockTask>();
        //                List<Location> updateList = new List<Location>();

        //                //库存状态修改
        //                inventoryEntity.Status = 2;
        //                await _inventoryApp.UpdateForTrackedAsync(inventoryEntity, false);
        //                Location NowLocation = await _locationApp.GetByIdAsync(inventoryEntity.LocationId);
        //                //修改取货的库位状态
        //                NowLocation.LocationStatus = LocationStatus.锁定;
        //                NowLocation.Area = null;
        //                NowLocation.Equipment = null;
        //                NowLocation.Warehouse = null;

        //                await _locationApp.UpdateForNotTrackedAsync(NowLocation, false);
        //                string entityCode = "";
        //                for (var rc = 1; rc <= RemoveCount; rc++)
        //                {
        //                    StockTask entity = new StockTask();
        //                    entity.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //                    entity.WarehouseId = _appConfiguration.Value.WarehouseId;
        //                    entity.AreaId = inventoryEntity.AreaId;
        //                    entity.AreaName = inventoryEntity.AreaName;
        //                    entity.TaskType = TaskType.移库;
        //                    entity.IsRepair = false;
        //                    entity.TaskStatus = TaskStatus.等待执行;
        //                    entity.Priority = TaskPriority.普通;
        //                    entity.CarTypeNum = inventoryEntity.CarTypeNum;
        //                    entity.CarTypeFace = inventoryEntity.CarTypeFace;
        //                    entity.CarTypeId = (int)inventoryEntity.CarTypeId;
        //                    entity.CarTypeName = inventoryEntity.CarTypeName;
        //                    entity.CarTypeInt = inventoryEntity.CarTypeInt;
        //                    entity.CreateTime = DateTime.Now;
        //                    entity.JXCarTypeNum = inventoryEntity.JXCarTypeNum;
        //                    entity.SetTaskType = 0;
        //                    entity.TaskNo = await _tempCodeApp.GetNewCode();
        //                    entity.OutLocationId = NowLocation.Id;
        //                    entity.OutLocationCode = NowLocation.Code;
        //                    //随机生成库位
        //                    //int r = new Random().Next(locationList.Count);
        //                    Location inLocaiton = null;
        //                    if (rc % 2 != 0)
        //                    {
        //                        //循环次数为奇数的话,也就是从起始位置到最大列数
        //                        inLocaiton = MaxFloor[0];
        //                        entity.InLocationId = inLocaiton.Id;
        //                        entity.InLocationCode = inLocaiton.Code;
        //                    }
        //                    else if (rc % 2 == 0)
        //                    {
        //                        //循环次数为偶数的话,也就是从目的位置到最小列数
        //                        inLocaiton = MinFloor[0];
        //                        entity.InLocationId = inLocaiton.Id;
        //                        entity.InLocationCode = inLocaiton.Code;
        //                    }
        //                    //Location inLocaiton = MaxFloor[0];
        //                    //entity.InLocationId = inLocaiton.Id;
        //                    //entity.InLocationCode = inLocaiton.Code;

        //                    entity.LocationType = 0;
        //                    entity.EquipmentId = inLocaiton.EquipmentId;
        //                    entity.EquipmentName = inLocaiton.Equipment == null ? "" : inLocaiton.Equipment.Name;
        //                    if (rc == 1)
        //                    {
        //                        entityCode = entity.Code;
        //                        entity.Remark = "手动下发移库任务。";
        //                    }
        //                    else
        //                    {
        //                        entity.Remark = entityCode + "移库任务复制任务。";
        //                    }


        //                    //修改放货的库位状态
        //                    inLocaiton.LocationStatus = LocationStatus.锁定;
        //                    inLocaiton.Area = null;
        //                    inLocaiton.Equipment = null;
        //                    inLocaiton.Warehouse = null;
        //                    await _locationApp.UpdateForNotTrackedAsync(inLocaiton, false);
        //                    await Repository.AddAsync(entity, false);
        //                    await Repository.SaveChangesAsync();

        //                    NowLocation = inLocaiton;
        //                }
        //            }
        //            else
        //            {
        //                //除了主线库以外其他夹具库

        //                //获取同个区域的未禁用，未使用的库位
        //                QueryLocationReq queryLocationReq = new QueryLocationReq() { IsDisabled = false, Status = 0, IsKukou = false };
        //                var locationList = await _locationApp.GetList(queryLocationReq);
        //                var MaxCloumn = locationList.OrderByDescending(s => s.ColumnNo).Take(5).ToList(); //查询集合对象中某库位最大的列数按倒序排列，并且返回前五条数据
        //                var MaxFloor = MaxCloumn.OrderByDescending(s => s.FloorNo).Take(1).ToList(); //根据查询集合对象中某库位最大的列数再次根据最大层数排列，并且返回第一条数据(最终要到的列数)
        //                var MinCloumn = locationList.OrderBy(t => t.ColumnNo).Take(5).ToList(); //根据查询集合对象中某库位最小的列数排序，并且返回前五条数据
        //                var MinFloor = MinCloumn.OrderBy(t => t.FloorNo).Take(1).ToList(); //根据查询集合对象中某库位最小的列数排序再次根据最小层数排列，并且返回第一条数据(最终要得到的最小列数)
        //                List<StockTask> addList = new List<StockTask>();
        //                List<Location> updateList = new List<Location>();

        //                //库存状态修改
        //                inventoryEntity.Status = 2;
        //                await _inventoryApp.UpdateForTrackedAsync(inventoryEntity, false);
        //                Location NowLocation = await _locationApp.GetByIdAsync(inventoryEntity.LocationId);
        //                //修改取货的库位状态
        //                NowLocation.LocationStatus = LocationStatus.锁定;
        //                NowLocation.Area = null;
        //                NowLocation.Equipment = null;
        //                NowLocation.Warehouse = null;

        //                await _locationApp.UpdateForNotTrackedAsync(NowLocation, false);
        //                string entityCode = "";
        //                for (var rc = 1; rc <= RemoveCount; rc++)
        //                {
        //                    StockTask entity = new StockTask();
        //                    entity.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //                    entity.WarehouseId = _appConfiguration.Value.WarehouseId;
        //                    entity.AreaId = inventoryEntity.AreaId;
        //                    entity.AreaName = inventoryEntity.AreaName;
        //                    entity.TaskType = TaskType.移库;
        //                    entity.IsRepair = false;
        //                    entity.TaskStatus = TaskStatus.等待执行;
        //                    entity.Priority = TaskPriority.普通;
        //                    entity.CarTypeNum = inventoryEntity.CarTypeNum;
        //                    entity.CarTypeFace = inventoryEntity.CarTypeFace;
        //                    entity.CarTypeId = (int)inventoryEntity.CarTypeId;
        //                    entity.CarTypeName = inventoryEntity.CarTypeName;
        //                    entity.CarTypeInt = inventoryEntity.CarTypeInt;
        //                    entity.CreateTime = DateTime.Now;
        //                    entity.SetTaskType = 0;
        //                    entity.TaskNo = await _tempCodeApp.GetNewCode();


        //                    entity.OutLocationId = NowLocation.Id;
        //                    entity.OutLocationCode = NowLocation.Code;

        //                    //int r = new Random().Next(locationList.Count);
        //                    //Location inLocaiton = locationList[r];
        //                    Location inLocaiton = null;
        //                    if (rc % 2 != 0)
        //                    {
        //                        //循环次数为奇数的话,也就是从起始位置到最大列数
        //                        inLocaiton = MaxFloor[0];
        //                        entity.InLocationId = inLocaiton.Id;
        //                        entity.InLocationCode = inLocaiton.Code;
        //                    }
        //                    else if (rc % 2 == 0)
        //                    {
        //                        //循环次数为偶数的话,也就是从目的位置到最小列数
        //                        inLocaiton = MinFloor[0];
        //                        entity.InLocationId = inLocaiton.Id;
        //                        entity.InLocationCode = inLocaiton.Code;
        //                    }

        //                    entity.InLocationId = inLocaiton.Id;
        //                    entity.InLocationCode = inLocaiton.Code;

        //                    entity.LocationType = 0;
        //                    entity.EquipmentId = inLocaiton.EquipmentId;
        //                    entity.EquipmentName = inLocaiton.Equipment == null ? "" : inLocaiton.Equipment.Name;
        //                    if (rc == 1)
        //                    {
        //                        entityCode = entity.Code;
        //                        entity.Remark = "手动下发移库任务。";
        //                    }
        //                    else
        //                    {
        //                        entity.Remark = entityCode + "移库任务复制任务。";
        //                    }

        //                    //修改放货的库位状态
        //                    inLocaiton.LocationStatus = LocationStatus.锁定;
        //                    inLocaiton.Area = null;
        //                    inLocaiton.Equipment = null;
        //                    inLocaiton.Warehouse = null;
        //                    await _locationApp.UpdateForNotTrackedAsync(inLocaiton, false);
        //                    await Repository.AddAsync(entity, false);
        //                    await Repository.SaveChangesAsync();
        //                    NowLocation = inLocaiton;
        //                }
        //            }
        //            //获取同个区域的未禁用，未使用的库位
        //            //QueryLocationReq queryLocationReq = new QueryLocationReq() { IsDisabled = false, AreaId = inventoryEntity.AreaId, Status = 0 , IsKukou =false};
        //            //var locationList = await _locationApp.GetList(queryLocationReq);


        //            //List<StockTask> addList = new List<StockTask>();
        //            //List<Location> updateList = new List<Location>();

        //            ////库存状态修改
        //            //inventoryEntity.Status = 2;
        //            //await _inventoryApp.UpdateForTrackedAsync(inventoryEntity, false);
        //            //Location NowLocation = await _locationApp.GetByIdAsync(inventoryEntity.LocationId);
        //            ////修改取货的库位状态
        //            //NowLocation.LocationStatus = LocationStatus.锁定;
        //            //NowLocation.Area = null;
        //            //NowLocation.Equipment = null;
        //            //NowLocation.Warehouse = null;

        //            //await _locationApp.UpdateForNotTrackedAsync(NowLocation, false);
        //            //string entityCode = "";
        //            //for (var rc = 1; rc <= RemoveCount; rc++)
        //            //{

        //            //    StockTask entity = new StockTask();
        //            //    entity.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //            //    entity.WarehouseId = _appConfiguration.Value.WarehouseId;
        //            //    entity.AreaId = inventoryEntity.AreaId;
        //            //    entity.AreaName = inventoryEntity.AreaName;
        //            //    entity.TaskType = TaskType.移库;
        //            //    entity.IsRepair = false;
        //            //    entity.TaskStatus = Entities.TaskStatus.等待执行;
        //            //    entity.Priority = TaskPriority.普通;
        //            //    entity.CarTypeNum = inventoryEntity.CarTypeNum;
        //            //    entity.CarTypeFace = inventoryEntity.CarTypeFace;
        //            //    entity.CarTypeId = (int)inventoryEntity.CarTypeId;
        //            //    entity.CarTypeName = inventoryEntity.CarTypeName;
        //            //    entity.CarTypeInt = inventoryEntity.CarTypeInt;
        //            //    entity.CreateTime = DateTime.Now;
        //            //    entity.SetTaskType = 0;
        //            //    entity.TaskNo = await _tempCodeApp.GetNewCode();


        //            //    entity.OutLocationId = NowLocation.Id;
        //            //    entity.OutLocationCode = NowLocation.Code;

        //            //    int r = new Random().Next(locationList.Count);
        //            //    Location inLocaiton = locationList[r];
        //            //    entity.InLocationId = inLocaiton.Id;
        //            //    entity.InLocationCode = inLocaiton.Code;

        //            //    entity.LocationType = 0;
        //            //    entity.EquipmentId = inLocaiton.EquipmentId;
        //            //    entity.EquipmentName = inLocaiton.Equipment==null? "": inLocaiton.Equipment.Name;
        //            //    if (rc == 1)
        //            //    {
        //            //        entityCode = entity.Code;
        //            //        entity.Remark = "手动下发移库任务。";
        //            //    }
        //            //    else
        //            //    {
        //            //        entity.Remark = entityCode + "移库任务复制任务。";
        //            //    }


        //            //    //修改放货的库位状态
        //            //    inLocaiton.LocationStatus = LocationStatus.锁定;
        //            //    inLocaiton.Area = null;
        //            //    inLocaiton.Equipment = null;
        //            //    inLocaiton.Warehouse = null;
        //            //    await _locationApp.UpdateForNotTrackedAsync(inLocaiton, false);
        //            //    await this.Repository.AddAsync(entity, false);
        //            // await this.Repository.SaveChangesAsync();

        //            //    NowLocation = inLocaiton;


        //            //}
        //            transaction.Commit();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            return false;
        //        }
        //    }
        //}


        ///// <summary>
        /////手动更新任务的优先级
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<bool> UpdatePriority(UpdatePriorityInput input)
        //{
        //    var query = new Specification<StockTask>(a => !a.IsDeleted && input.SelectIds.Contains(a.Id));
        //    var currentWarehouseId = _appConfiguration.Value.WarehouseId;
        //    query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
        //    var list = await Repository.Query(query).ToListAsync();
        //    foreach (var item in list)
        //    {
        //        item.Priority = (TaskPriority)input.Priority;
        //    }

        //    await Repository.UpdateRangeAsync(list, true);
        //    await Repository.SaveChangesAsync();
        //    return true;
        //}

        ///// <summary>
        ///// 手动更新任务状态
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<bool> UpdateTaskStatus(UpdateTaskStatusInput input)
        //{
        //    var query = new Specification<StockTask>(a => !a.IsDeleted && input.SelectIds.Contains(a.Id));
        //    var currentWarehouseId = _appConfiguration.Value.WarehouseId;
        //    query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
        //    var list = await Repository.Query(query).AsNoTracking().ToListAsync();


        //    var result = false;
        //    foreach (var item in list)
        //    {
        //        item.TaskStatus = (Entities.TaskStatus)input.TaskStatus;
        //        if (item.TaskStatus == Entities.TaskStatus.正在执行)
        //        {
        //            result = await this.PLC_UpdateStockTask(item.TaskNo, Entities.TaskStatus.正在执行, 0);
        //        }
        //        else if (item.TaskStatus == Entities.TaskStatus.任务完成)
        //        {
        //            result = await this.PLC_UpdateStockTask(item.TaskNo, Entities.TaskStatus.任务完成, 0);
        //        }
        //        else if (item.TaskStatus == Entities.TaskStatus.取消任务)
        //        {
        //            result = await this.PLC_UpdateStockTask(item.TaskNo, Entities.TaskStatus.取消任务, 0);
        //        }
        //        else
        //        {
        //            item.Remark += "手动修改任务状态为" + item.TaskStatus.ToString() + "。";
        //            await Repository.UpdateAsync(item, true);
        //            result = true;
        //        }
        //    }

        //    return result;
        //}



        ///// <summary>
        /////根据plc状态自动更新任务状态
        ///// </summary>
        ///// <param name="taskNo">任务号</param>
        ///// <param name="taskStatus">任务状态</param>
        ///// <param name="setTaskType">0手动  1自动</param>
        ///// <returns></returns>
        //public async Task<bool> PLC_UpdateStockTask(int taskNo, TaskStatus taskStatus, int setTaskType)
        //{
        //    using (var transaction = _dBContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var status = taskStatus;

        //            //根据任务号获取TaskEntity
        //            var query = new Specification<StockTask>(a => !a.IsDeleted && a.TaskNo == taskNo);
        //            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
        //            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
        //            var entity = await Repository.Query(query).AsNoTracking().FirstOrDefaultAsync();

        //            if (entity != null)
        //            {
        //                entity.TaskStatus = status;
        //                entity.SetTaskType = setTaskType;
        //                if (status == Entities.TaskStatus.任务完成)
        //                {
        //                    if (setTaskType == 0)
        //                    {
        //                        entity.Remark += "手动完成任务。";
        //                    }

        //                    entity.CompleteTime = DateTime.Now;

        //                    #region 处理库存

        //                    if (entity.TaskType == Entities.TaskType.入库)
        //                    {
        //                        #region 入库任务完成 添加库存信息

        //                        Inventory inventoryEntity = new Inventory();
        //                        inventoryEntity.WarehouseId = entity.WarehouseId;
        //                        inventoryEntity.AreaId = entity.AreaId;
        //                        inventoryEntity.AreaName = entity.AreaName;
        //                        inventoryEntity.LocationId = (int)entity.InLocationId;
        //                        inventoryEntity.LocationCode = entity.InLocationCode;
        //                        inventoryEntity.CarTypeId = entity.CarTypeId;
        //                        inventoryEntity.CarTypeName = entity.CarTypeName;
        //                        inventoryEntity.CarTypeNum = entity.CarTypeNum;
        //                        inventoryEntity.CarTypeFace = entity.CarTypeFace;
        //                        inventoryEntity.CarTypeInt = entity.CarTypeInt;
        //                        inventoryEntity.CreateTime = DateTime.Now;
        //                        inventoryEntity.Status = 1;
        //                        inventoryEntity.SetTaskType = setTaskType;
        //                        if (currentWarehouseId == 1)
        //                        {
        //                            //只有主线库才有plc编号转机械编号
        //                            inventoryEntity.JXCarTypeNum = entity.JXCarTypeNum;
        //                        }
        //                        else
        //                        {
        //                            inventoryEntity.JXCarTypeNum = entity.JXCarTypeNum;//其他库作为十进制RFID使用
        //                        }


        //                        await _inventoryApp.AddAsync(inventoryEntity);

        //                        #endregion
        //                    }
        //                    else if (entity.TaskType == Entities.TaskType.出库 || entity.TaskType == Entities.TaskType.存车修正)
        //                    {
        //                        #region 删除库存

        //                        GetInventoryListInput getInventoryListInput = new GetInventoryListInput()
        //                        {
        //                            LocationId = entity.OutLocationId
        //                        };
        //                        var inventoryEntity = await _inventoryApp.GetInventoryEntity(getInventoryListInput);
        //                        if (inventoryEntity != null)
        //                        {
        //                            await _inventoryApp.DeleteForNotTrakedAsync(inventoryEntity.Id, false);
        //                        }

        //                        #endregion
        //                    }
        //                    else if (entity.TaskType == Entities.TaskType.移库)
        //                    {
        //                        #region 修改库存

        //                        GetInventoryListInput getInventoryListInput = new GetInventoryListInput()
        //                        {
        //                            LocationId = entity.OutLocationId
        //                        };
        //                        var inventoryEntity = await _inventoryApp.GetInventoryEntity(getInventoryListInput);
        //                        if (inventoryEntity != null)
        //                        {
        //                            inventoryEntity.LocationId = (int)entity.InLocationId;
        //                            inventoryEntity.LocationCode = entity.InLocationCode;
        //                            await _inventoryApp.UpdateForTrackedAsync(inventoryEntity, false);
        //                        }

        //                        #endregion
        //                    }

        //                    #endregion


        //                    #region 新增StockTaskHistory

        //                    var historyEntity = new StockTaskHistory()
        //                    {
        //                        Code = entity.Code,
        //                        WarehouseId = entity.WarehouseId,
        //                        AreaId = entity.AreaId,
        //                        AreaName = entity.AreaName,
        //                        TaskType = entity.TaskType,
        //                        IsRepair = entity.IsRepair,
        //                        TaskStatus = entity.TaskStatus,
        //                        Priority = entity.Priority,
        //                        CarTypeNum = entity.CarTypeNum,
        //                        CarTypeFace = entity.CarTypeFace,
        //                        CarTypeId = entity.CarTypeId,
        //                        CarTypeName = entity.CarTypeName,
        //                        CarTypeInt = entity.CarTypeInt,
        //                        CreateTime = entity.CreateTime,
        //                        StartTime = entity.StartTime,
        //                        CompleteTime = DateTime.Now,
        //                        LocationType = entity.LocationType,
        //                        GatewayId = entity.GatewayId,
        //                        GatewayName = entity.GatewayName,
        //                        EquipmentId = entity.EquipmentId,
        //                        EquipmentName = entity.EquipmentName,
        //                        TaskNo = entity.TaskNo,
        //                        OutLocationId = entity.OutLocationId,
        //                        OutLocationCode = entity.OutLocationCode,
        //                        InLocationId = entity.InLocationId,
        //                        InLocationCode = entity.InLocationCode,
        //                        SetTaskType = setTaskType,
        //                        WarnContent = entity.WarnContent,
        //                        Remark = entity.Remark,
        //                        RequestTaskTime = entity.RequestTaskTime,
        //                        JXCarTypeNum = entity.JXCarTypeNum
        //                    };

        //                    await _dBContext.AddAsync(historyEntity);

        //                    #endregion

        //                    #region 在StackTask删除 TaskEntity

        //                    entity.IsDeleted = true;
        //                    await UpdateForTrackedAsync(entity, false);

        //                    #endregion


        //                    #region 修改库位状态

        //                    if (entity.TaskType == Entities.TaskType.入库)
        //                    {
        //                        var locationEntity = await _locationApp.GetByIdAsync((int)entity.InLocationId);
        //                        locationEntity.LocationStatus = LocationStatus.已使用;
        //                        _dBContext.Update<Location>(locationEntity);
        //                    }
        //                    else if (entity.TaskType == Entities.TaskType.出库 || entity.TaskType == Entities.TaskType.存车修正)
        //                    {
        //                        var locationEntity = await _locationApp.GetByIdAsync((int)entity.OutLocationId);
        //                        locationEntity.LocationStatus = LocationStatus.未使用;
        //                        _dBContext.Update<Location>(locationEntity);
        //                    }
        //                    else if (entity.TaskType == Entities.TaskType.移库)
        //                    {
        //                        if (entity.Remark.Contains("出库"))
        //                        {
        //                            //由出库任务产生的移库任务  
        //                            var locationEntity = await _locationApp.GetByIdAsync((int)entity.InLocationId);
        //                            locationEntity.LocationStatus = LocationStatus.已使用;
        //                            _dBContext.Update<Location>(locationEntity);
        //                        }
        //                        else if (entity.Remark.Contains("入库"))
        //                        {
        //                            //由入库任务产生的移库任务
        //                            var b = entity.Remark.Contains("入库");
        //                            if (b)
        //                            {
        //                                var locationEntity = await _locationApp.GetByIdAsync((int)entity.OutLocationId);
        //                                locationEntity.LocationStatus = LocationStatus.未使用;
        //                                _dBContext.Update<Location>(locationEntity);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var locationEntity = await _locationApp.GetByIdAsync((int)entity.OutLocationId);
        //                            locationEntity.LocationStatus = LocationStatus.未使用;
        //                            _dBContext.Update<Location>(locationEntity);

        //                            var locationEntity2 = await _locationApp.GetByIdAsync((int)entity.InLocationId);
        //                            locationEntity2.LocationStatus = LocationStatus.已使用;
        //                            _dBContext.Update<Location>(locationEntity2);
        //                        }
        //                    }

        //                    #endregion
        //                }
        //                else if (status == Entities.TaskStatus.正在执行)
        //                {
        //                    entity.StartTime = DateTime.Now;
        //                    if (setTaskType == 0)
        //                    {
        //                        entity.Remark += "手动开始任务。";
        //                    }

        //                    #region 修改TaskEntity

        //                    await Repository.UpdateAsync(entity, true);

        //                    #endregion
        //                }
        //                else if (status == Entities.TaskStatus.取消任务)
        //                {
        //                    if (setTaskType == 0)
        //                    {
        //                        entity.Remark += "手动取消任务。";
        //                    }

        //                    #region 将任务删除，状态为取消任务

        //                    entity.IsDeleted = true;
        //                    await Repository.UpdateAsync(entity, false);

        //                    #endregion

        //                    #region 新增StockTaskHistory

        //                    var historyEntity = new StockTaskHistory()
        //                    {
        //                        Code = entity.Code,
        //                        WarehouseId = entity.WarehouseId,
        //                        AreaId = entity.AreaId,
        //                        AreaName = entity.AreaName,
        //                        TaskType = entity.TaskType,
        //                        IsRepair = entity.IsRepair,
        //                        TaskStatus = entity.TaskStatus,
        //                        Priority = entity.Priority,
        //                        CarTypeNum = entity.CarTypeNum,
        //                        CarTypeFace = entity.CarTypeFace,
        //                        CarTypeId = entity.CarTypeId,
        //                        CarTypeName = entity.CarTypeName,
        //                        CarTypeInt = entity.CarTypeInt,
        //                        CreateTime = entity.CreateTime,
        //                        StartTime = entity.StartTime,
        //                        CompleteTime = entity.CompleteTime,
        //                        LocationType = entity.LocationType,
        //                        GatewayId = entity.GatewayId,
        //                        GatewayName = entity.GatewayName,
        //                        EquipmentId = entity.EquipmentId,
        //                        EquipmentName = entity.EquipmentName,
        //                        TaskNo = entity.TaskNo,
        //                        OutLocationId = entity.OutLocationId,
        //                        OutLocationCode = entity.OutLocationCode,
        //                        InLocationId = entity.InLocationId,
        //                        InLocationCode = entity.InLocationCode,
        //                        SetTaskType = entity.SetTaskType,
        //                        WarnContent = entity.WarnContent,
        //                        Remark = entity.Remark,
        //                        RequestTaskTime = entity.RequestTaskTime,
        //                        JXCarTypeNum = entity.JXCarTypeNum
        //                    };

        //                    await _dBContext.AddAsync(historyEntity);

        //                    #endregion

        //                    //修改库位状态                           
        //                    if (entity.TaskType == Entities.TaskType.入库)
        //                    {
        //                        var locationEntity = await _locationApp.GetByIdAsync((int)entity.InLocationId);
        //                        locationEntity.LocationStatus = LocationStatus.未使用;
        //                        _dBContext.Update<Location>(locationEntity);
        //                    }
        //                    else if (entity.TaskType == Entities.TaskType.出库 || entity.TaskType == Entities.TaskType.存车修正)
        //                    {
        //                        var locationEntity = await _locationApp.GetByIdAsync((int)entity.OutLocationId);
        //                        locationEntity.LocationStatus = LocationStatus.已使用;
        //                        _dBContext.Update<Location>(locationEntity);

        //                        //修改库存状态
        //                        GetInventoryListInput getInventoryListInput = new GetInventoryListInput()
        //                        {
        //                            CarTypeNum = entity.CarTypeNum
        //                        };
        //                        var inventryEntity = await _inventoryApp.GetInventoryEntity(getInventoryListInput);
        //                        inventryEntity.Status = 1;
        //                        _dBContext.Update(inventryEntity);
        //                    }
        //                    else if (entity.TaskType == Entities.TaskType.移库)
        //                    {
        //                        //修改库存状态
        //                        GetInventoryListInput getInventoryListInput = new GetInventoryListInput()
        //                        {
        //                            CarTypeNum = entity.CarTypeNum
        //                        };
        //                        var inventryEntity = await _inventoryApp.GetInventoryEntity(getInventoryListInput);


        //                        var locationEntity = await _locationApp.GetByIdAsync((int)entity.OutLocationId);
        //                        if (inventryEntity.LocationId == entity.OutLocationId)
        //                        {
        //                            locationEntity.LocationStatus = LocationStatus.已使用;
        //                        }
        //                        else
        //                        {
        //                            locationEntity.LocationStatus = LocationStatus.未使用;
        //                        }

        //                        _dBContext.Update<Location>(locationEntity);

        //                        var locationEntity2 = await _locationApp.GetByIdAsync((int)entity.InLocationId);
        //                        locationEntity2.LocationStatus = LocationStatus.未使用;
        //                        _dBContext.Update<Location>(locationEntity2);

        //                        if (inventryEntity.Status != 1)
        //                        {
        //                            inventryEntity.Status = 1;
        //                            _dBContext.Update(inventryEntity);
        //                        }
        //                    }
        //                }


        //                await Repository.SaveChangesAsync();
        //                transaction.Commit();
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var a = ex.Message;
        //            transaction.Rollback();
        //            return false;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 任务已执行，取消任务，添加移库任务，将台车退回取货位置
        ///// </summary>
        ///// <param name="TaskEntity"></param>
        ///// <returns></returns>
        //public async Task<bool> FinishedTaskAndAddMoveTask(StockTask TaskEntity)
        //{
        //    //取消当前任务，生成移库任务【库存不用修改】
        //    using (var transaction = _dBContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            //注意：库存数据因为任务没有完成所以库存数据没有修改，库位的状态在移库任务完成之后恢复

        //            #region 将任务删除，状态为取消任务

        //            TaskEntity.TaskStatus = Entities.TaskStatus.取消任务;
        //            TaskEntity.Remark += "任务已执行，需要退回，手动取消任务。";
        //            TaskEntity.SetTaskType = 1;
        //            TaskEntity.IsDeleted = true;
        //            await Repository.UpdateAsync(TaskEntity, false);

        //            #endregion

        //            #region 新增StockTaskHistory

        //            var historyEntity = new StockTaskHistory()
        //            {
        //                Code = TaskEntity.Code,
        //                WarehouseId = TaskEntity.WarehouseId,
        //                AreaId = TaskEntity.AreaId,
        //                AreaName = TaskEntity.AreaName,
        //                TaskType = TaskEntity.TaskType,
        //                IsRepair = TaskEntity.IsRepair,
        //                TaskStatus = TaskEntity.TaskStatus,
        //                Priority = TaskEntity.Priority,
        //                CarTypeNum = TaskEntity.CarTypeNum,
        //                CarTypeFace = TaskEntity.CarTypeFace,
        //                CarTypeId = TaskEntity.CarTypeId,
        //                CarTypeName = TaskEntity.CarTypeName,
        //                CarTypeInt = TaskEntity.CarTypeInt,
        //                CreateTime = TaskEntity.CreateTime,
        //                StartTime = TaskEntity.StartTime,
        //                CompleteTime = TaskEntity.CompleteTime,
        //                LocationType = TaskEntity.LocationType,
        //                GatewayId = TaskEntity.GatewayId,
        //                GatewayName = TaskEntity.GatewayName,
        //                EquipmentId = TaskEntity.EquipmentId,
        //                EquipmentName = TaskEntity.EquipmentName,
        //                TaskNo = TaskEntity.TaskNo,
        //                OutLocationId = TaskEntity.OutLocationId,
        //                OutLocationCode = TaskEntity.OutLocationCode,
        //                InLocationId = TaskEntity.InLocationId,
        //                InLocationCode = TaskEntity.InLocationCode,
        //                SetTaskType = TaskEntity.SetTaskType,
        //                WarnContent = TaskEntity.WarnContent,
        //                Remark = TaskEntity.Remark,
        //                RequestTaskTime = TaskEntity.RequestTaskTime
        //            };

        //            await _dBContext.AddAsync(historyEntity);

        //            #endregion

        //            #region 添加移库任务

        //            StockTask entity = new StockTask();
        //            entity.Code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //            entity.WarehouseId = _appConfiguration.Value.WarehouseId;
        //            entity.AreaId = TaskEntity.AreaId;
        //            entity.AreaName = TaskEntity.AreaName;
        //            entity.TaskType = TaskType.移库;
        //            entity.Remark = "由" + TaskEntity.TaskType.ToString() + "任务号：" + TaskEntity.TaskNo + "退回产生的移库任务。";
        //            entity.TaskStatus = Entities.TaskStatus.等待执行;
        //            entity.Priority = TaskPriority.普通;
        //            entity.CarTypeNum = TaskEntity.CarTypeNum;
        //            entity.CarTypeFace = TaskEntity.CarTypeFace;
        //            entity.CarTypeId = TaskEntity.Id;
        //            entity.CarTypeName = TaskEntity.CarTypeName;
        //            entity.CreateTime = DateTime.Now;
        //            entity.SetTaskType = 0;

        //            entity.EquipmentId = TaskEntity.EquipmentId;
        //            entity.EquipmentName = TaskEntity.EquipmentName;
        //            entity.TaskNo = await _tempCodeApp.GetNewCode();
        //            if (TaskEntity.TaskType == TaskType.入库)
        //            {
        //                entity.OutLocationId = TaskEntity.InLocationId;
        //                entity.OutLocationCode = TaskEntity.InLocationCode;
        //                var gateWayEntity = await _gatewayApp.GetByIdAsNoTrackingAsync((int)TaskEntity.GatewayId);
        //                entity.InLocationId = gateWayEntity.LocationId;
        //                entity.InLocationCode = gateWayEntity.LocationCode;
        //                entity.LocationType = TaskEntity.LocationType;
        //            }
        //            else if (TaskEntity.TaskType == TaskType.出库)
        //            {
        //                var gateWayEntity = await _gatewayApp.GetByIdAsNoTrackingAsync((int)TaskEntity.GatewayId);
        //                entity.OutLocationCode = gateWayEntity.LocationCode;
        //                entity.OutLocationId = gateWayEntity.LocationId;

        //                entity.InLocationId = TaskEntity.OutLocationId;
        //                entity.InLocationCode = TaskEntity.OutLocationCode;
        //                entity.LocationType = TaskEntity.LocationType;
        //            }

        //            var res = await Repository.AddAsync(entity, false);

        //            #endregion

        //            await Repository.SaveChangesAsync();
        //            transaction.Commit();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            var a = ex.Message;
        //            transaction.Rollback();
        //            return false;
        //        }
        //    }
        //}


        #endregion
        /// <summary>
        /// 主线库将手动入库的RFID转成线体传来的PLC编号(数据库存的就是PLC编号)(方便人工操作);
        /// </summary>
        /// <param name="CarTypeNum"></param>
        /// <param name="GateID"></param>
        /// <returns></returns>
        //public async Task<string> ShouDongRukuRfid(string InputCarTypeNum, int GatewayID)
        //{
        //    var Rfid = "0";
        //    //通过出入口ID判断是主线还是UB(手动入库的话,只有入口)
        //    if (GatewayID == 1 || GatewayID == 2 || GatewayID == 3)
        //    {
        //        //这是UB区域
        //        if (InputCarTypeNum.Length == 4)
        //        {
        //            var rfidBef = InputCarTypeNum.Substring(0, 1);
        //            var rfidAft = InputCarTypeNum.Substring(1, 3);
        //            var r1 = int.Parse(rfidBef);
        //            var r2 = int.Parse(rfidAft);
        //            var B1 = Convert.ToString(r1, 2).PadLeft(8, '0');
        //            var B2 = Convert.ToString(r2, 2).PadLeft(8, '0');
        //            string res = B1 + B2;
        //            Rfid = Convert.ToInt32(res, 2).ToString();
        //        }
        //        else
        //        {
        //            //五位数才是正常的UB台车RFID
        //            var rfidBef = InputCarTypeNum.Substring(0, 2);
        //            var rfidAft = InputCarTypeNum.Substring(2, 3);
        //            var r1 = int.Parse(rfidBef);
        //            var r2 = int.Parse(rfidAft);
        //            var B1 = Convert.ToString(r1, 2).PadLeft(8, '0');
        //            var B2 = Convert.ToString(r2, 2).PadLeft(8, '0');
        //            string res = B1 + B2;
        //            Rfid = Convert.ToInt32(res, 2).ToString();
        //        }
        //    }
        //    else
        //    {
        //        //主线区域
        //        if (InputCarTypeNum.Length == 4)
        //        {
        //            var rfidBef = InputCarTypeNum.Substring(0, 1);
        //            var rfidAft = InputCarTypeNum.Substring(1, 3);
        //            var r1 = int.Parse(rfidBef);
        //            var r2 = int.Parse(rfidAft);
        //            var B1 = Convert.ToString(r1, 2).PadLeft(8, '0');
        //            var B2 = Convert.ToString(r2, 2).PadLeft(8, '0');
        //            string res = B1 + B2;
        //            Rfid = Convert.ToInt32(res, 2).ToString();
        //        }
        //    }

        //    return Rfid.ToString();
        //}
    }
}