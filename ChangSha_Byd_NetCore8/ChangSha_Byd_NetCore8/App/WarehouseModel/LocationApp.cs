using AsZero.DbContexts;
using ChangSha_Byd_NetCore8.App.Request;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using ChangSha_Byd_NetCore8.Protocols.Common;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChangSha_Byd_NetCore8.App.WarehouseModel
{
    public class LocationApp : FutureBaseEntityService<int, Location>
    {
        public readonly AsZeroDbContext _dBContext;
        private readonly IOptions<AppSetting> _appConfiguration;
        public LocationApp(IGenericRepository<int, Location> repo, AsZeroDbContext dBContext, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _dBContext = dBContext;
            _appConfiguration = appConfiguration;
        }
        public async Task<TableData> Load(QueryLocationReq request)
        {

            var query = new Specification<Location>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);

            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            if (request.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == request.AreaId);
            }
            if (request.Status != null)
            {
                query.CombineCritia(u => (int)u.LocationStatus == request.Status);
            }
            if (request.IsDisabled != null)
            {
                query.CombineCritia(u => u.IsDisabled == request.IsDisabled);
            }
            if (request.Id != null)
            {
                query.CombineCritia(u => u.Id == request.Id);
            }
            if (request.IsKukou != null)
            {
                if (request.IsKukou == true)
                {
                    //只查询库位类型是库口的
                    query.CombineCritia(u => u.LocationType == LocationType.库口);
                }
                else
                {
                    query.CombineCritia(u => u.LocationType != LocationType.库口);
                }

            }

            var pageSpec = query.New().AddInclude(u => u.Warehouse).AddInclude(u => u.Area).AddInclude(u => u.Equipment).ApplyOrderByDescending(a => a.Code).ApplyPaging(new Pagination(request.page, request.limit));

            var (count, data) = await LoadPageAsNoTrackingAsync(query, pageSpec);
            return new TableData { count = count, data = data };

        }

        public async Task<IReadOnlyList<Location>> GetList(QueryLocationReq input)
        {

            var query = new Specification<Location>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (!string.IsNullOrEmpty(input.key))
            {
                query.CombineCritia(u => u.Code.Contains(input.key) || u.Name.Contains(input.key));
            }
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }
            if (input.Status != null)
            {
                query.CombineCritia(u => (int)u.LocationStatus == input.Status);
            }
            if (input.IsDisabled != null)
            {
                query.CombineCritia(u => u.IsDisabled == input.IsDisabled);
            }
            if (input.IsKukou != null)
            {
                if (input.IsKukou == true)
                {
                    //只查询库位类型是库口的
                    query.CombineCritia(u => u.LocationType == LocationType.库口);
                }
                else
                {
                    query.CombineCritia(u => u.LocationType != LocationType.库口);
                }

            }
            var list = await Repository.Query(query).Include(u => u.Warehouse).Include(u => u.Area).Include(u => u.Equipment).AsNoTracking().ToListAsync();

            return list;
        }


        public async Task<List<Location>> GetLocationList(QueryLocationReq input)
        {

            var query = new Specification<Location>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (!string.IsNullOrEmpty(input.key))
            {
                query.CombineCritia(u => u.Code.Contains(input.key) || u.Name.Contains(input.key));
            }
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }
            if (input.Status != null)
            {
                query.CombineCritia(u => (int)u.LocationStatus == input.Status);
            }
            if (input.IsDisabled != null)
            {
                query.CombineCritia(u => u.IsDisabled == input.IsDisabled);
            }
            if (input.IsKukou != null)
            {
                if (input.IsKukou == true)
                {
                    //只查询库位类型是库口的
                    query.CombineCritia(u => u.LocationType == LocationType.库口);
                }
                else
                {
                    query.CombineCritia(u => u.LocationType != LocationType.库口);
                }

            }
            var list = await Repository.Query(query).Include(u => u.Warehouse).Include(u => u.Area).Include(u => u.Equipment).ToListAsync();

            return list;
        }



        public async Task<Location> GetLocation(GetLocationInput input)
        {
            var query = new Specification<Location>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);

            if (input.Id != null)
            {
                query.CombineCritia(u => u.Id == input.Id);
            }
            if (!string.IsNullOrEmpty(input.Code))
            {
                query.CombineCritia(u => u.Code == input.Code);
            }
            return await Repository.Query(query).Include(a => a.Equipment).AsNoTracking().FirstOrDefaultAsync();
        }



        /// <summary>
        /// 入库分配库位，也就是排-行-层(这一步没看懂)
        /// </summary>
        /// <param name="area_CarType_GateWayEntity"></param>
        /// <returns></returns>
        public async Task<Location> GetInStockLocation(Area_CarType_GateWay area_CarType_GateWayEntity)
        {
            Location resLocaiton = null;
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            if (currentWarehouseId == 1)//侧围轮罩库
            {
                var query = new Specification<Location>(a => !a.IsDeleted && a.LocationStatus == LocationStatus.未使用 && a.IsDisabled == false && a.LocationType != LocationType.库口);
                query.CombineCritia(u => u.WarehouseId == currentWarehouseId);

                #region 超高夹具判断
                if (area_CarType_GateWayEntity.InGateway.Remark == "超高夹具")
                {
                    //一排31列之后是大库位
                    query.CombineCritia(t => t.LineNo == 1 && t.ColumnNo >= 31);//查询第一排的大库位区域从31列开始属于大库位
                    resLocaiton = await Repository.Query(query).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();
                }
                #endregion

                else
                {
                    //AB柱区域
                    if (area_CarType_GateWayEntity.AreaId == 1 || area_CarType_GateWayEntity.AreaId == 4)
                    {
                        query.CombineCritia(u => u.AreaId == area_CarType_GateWayEntity.AreaId);

                        //根据库口的列值开始从小到大。如果没有获取比库口小的列值
                        string InGateWayCode = area_CarType_GateWayEntity.InGateway.LocationCode;//排-列-层
                        var res = InGateWayCode.Split('-')[1];//获取列数
                        int inGateWayColumn = int.Parse(res);
                        //原程序
                        query.CombineCritia(u => u.ColumnNo >= inGateWayColumn);

                        //原程序
                        resLocaiton = await Repository.Query(query).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();
                        if (resLocaiton == null)
                        {
                            var query2 = new Specification<Location>(a => !a.IsDeleted && a.LocationStatus == LocationStatus.未使用 && a.IsDisabled == false && a.LocationType != LocationType.库口);
                            query2.CombineCritia(u => u.WarehouseId == currentWarehouseId);
                            query2.CombineCritia(u => u.AreaId == area_CarType_GateWayEntity.AreaId);
                            query2.CombineCritia(u => u.ColumnNo < inGateWayColumn);
                            resLocaiton = await Repository.Query(query2).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();
                        }
                    }
                    //轮罩区域库位放满可放AB柱区域
                    else if (area_CarType_GateWayEntity.AreaId==2 || area_CarType_GateWayEntity.AreaId==3)
                    {
                        query.CombineCritia(u => u.AreaId == area_CarType_GateWayEntity.AreaId);
                        //根据库口的列值开始从小到大。如果没有获取比库口小的列值
                        string InGateWayCode = area_CarType_GateWayEntity.InGateway.LocationCode;//排-列-层
                        var res = InGateWayCode.Split('-')[1];//获取列数
                        int inGateWayColumn = int.Parse(res);
                        //原程序
                        query.CombineCritia(u => u.ColumnNo >= inGateWayColumn);

                        //原程序
                        resLocaiton = await Repository.Query(query).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();
                        if (resLocaiton == null)
                        {
                            var query2 = new Specification<Location>(a => !a.IsDeleted && a.LocationStatus == LocationStatus.未使用 && a.IsDisabled == false && a.LocationType != LocationType.库口);
                            query2.CombineCritia(u => u.WarehouseId == currentWarehouseId);
                            query2.CombineCritia(u => u.AreaId == area_CarType_GateWayEntity.AreaId);
                            query2.CombineCritia(u => u.ColumnNo < inGateWayColumn);
                            resLocaiton = await Repository.Query(query2).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();


                            //使用共用区域
                            if (resLocaiton == null)
                            {
                                var querys = new Specification<Location>(a => a.WarehouseId == area_CarType_GateWayEntity.AreaId && !a.IsDisabled && a.LocationStatus == LocationStatus.未使用 && a.IsDisabled == false && a.LocationType != LocationType.库口);


                                if (area_CarType_GateWayEntity.AreaId == 2)
                                {
                                    //查询左AB柱区域中第8列之后库位
                                    querys.CombineCritia(a => a.AreaId == 1 && a.ColumnNo>=8 );
                                    resLocaiton = await Repository.Query(query).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();

                                }
                                else if (area_CarType_GateWayEntity.AreaId == 3)
                                {
                                    querys.CombineCritia(a => a.AreaId == 4 && a.ColumnNo >= 25 && a.ColumnNo<=29);
                                    resLocaiton = await Repository.Query(query).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();
                                }
                            }


                        }
                    }

                }


                //query.CombineCritia(u => u.AreaId == area_CarType_GateWayEntity.AreaId);
                //if (area_CarType_GateWayEntity.InGateway.Name.Contains("UB"))
                //{
                //    //ub是列值由小到大存放
                //    resLocaiton = await this.Repository.Query(query).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();
                //}
                //else
                //{
                //    //mb是列值由大到小存放
                //    resLocaiton = await this.Repository.Query(query).Include(a => a.Equipment).OrderByDescending(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();
                //}
            }
            else//只有两个，一个cw，一个QH
            {
                //TODO: 待完善
                //其他夹具库
                var query = new Specification<Location>(
                    a => !a.IsDeleted &&                        //都是0，都没有删除，不用看了
                    a.LocationStatus == LocationStatus.未使用 &&   //要求LocationStatus=0
                    a.IsDisabled == false &&                        //要求IsDisabled=0
                    a.LocationType != LocationType.库口      //不能是库口 也就是!=2
                );
                query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
                //因为分拼库夹具超高,只能放到大库位------>对应的好像是QH
                if (area_CarType_GateWayEntity.InGateway.Remark == "超高夹具")//没用到
                {
                }
                else//直接进else
                {
                    //TODO：有问题
                    query.CombineCritia(u => u.AreaId == area_CarType_GateWayEntity.AreaId);

                    //根据库口的列值开始从小到大。如果没有获取比库口小的列值
                    string InGateWayCode = area_CarType_GateWayEntity.InGateway.LocationCode;//排-列-层
                    var res = InGateWayCode.Split('-')[1];//获取列数
                    int inGateWayColumn = int.Parse(res);
                    //原程序
                    if (inGateWayColumn != 0)
                    {
                        query.CombineCritia(u => u.ColumnNo >= inGateWayColumn);
                    }
                    //按照列和层进行排序
                    resLocaiton = await this.Repository.Query(query).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();


                    if (resLocaiton == null)
                    {
                        var query2 = new Specification<Location>(a => !a.IsDeleted && a.LocationStatus == LocationStatus.未使用 && a.IsDisabled == false && a.LocationType != LocationType.库口);
                        query2.CombineCritia(u => u.WarehouseId == currentWarehouseId);
                        query2.CombineCritia(u => u.AreaId == area_CarType_GateWayEntity.AreaId);
                        query2.CombineCritia(u => u.ColumnNo < inGateWayColumn);
                        resLocaiton = await Repository.Query(query2).Include(a => a.Equipment).OrderBy(a => a.ColumnNo).ThenBy(a => a.FloorNo).AsNoTracking().FirstOrDefaultAsync();
                    }
                }
            }

            return resLocaiton;
        }


        public async Task<bool> IsHaveCode(string code)
        {
            var query = new Specification<Location>(a => !a.IsDeleted && a.Code == code);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var i = await Repository.Query(query).AsNoTracking().CountAsync();
            if (i > 0) { return true; }
            else { return false; }
        }

        /// <summary>
        /// 获取某个区域，某个排的库存俯视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetBoardOutput> GetBoard(GetBoardInput input)
        {
            GetBoardOutput result = new GetBoardOutput();
            var query = new Specification<Location>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }
            if (input.Line != null)
            {
                query.CombineCritia(u => u.LineNo == input.Line);
            }

            var lineList = await Repository.Query(query).AsNoTracking().ToListAsync();
            if (lineList != null && lineList.Count > 0)
            {
                if (currentWarehouseId == 1)//侧围轮罩库
                {
                    result.Floor1 = lineList.Where(a => a.FloorNo == 1).OrderBy(a => a.ColumnNo).ToList();
                    result.Floor2 = lineList.Where(a => a.FloorNo == 2).OrderBy(a => a.ColumnNo).ToList();
                    result.Floor3 = lineList.Where(a => a.FloorNo == 3).OrderBy(a => a.ColumnNo).ToList();
                    result.Floor4 = lineList.Where(a => a.FloorNo == 4).OrderBy(a => a.ColumnNo).ToList();
                    result.Floor5 = lineList.Where(a => a.FloorNo == 5).OrderBy(a => a.ColumnNo).ToList();

                }
                else if (currentWarehouseId == 2)//地板库
                {
                    result.Floor1 = lineList.Where(a => a.FloorNo == 1).OrderBy(a => a.ColumnNo).ToList();
                    result.Floor2 = lineList.Where(a => a.FloorNo == 2).OrderBy(a => a.ColumnNo).ToList();
                    result.Floor3 = lineList.Where(a => a.FloorNo == 3).OrderBy(a => a.ColumnNo).ToList();
                    result.Floor4 = lineList.Where(a => a.FloorNo == 4).OrderBy(a => a.ColumnNo).ToList();



                   // var floor1 = lineList.Where(a => a.FloorNo == 1).OrderBy(a => a.ColumnNo).ToList();
                   // List<Location> locationList1 = new List<Location>();

                   // for (var column = 1; column <= 46; column++)
                   // {

                   //     var loc = floor1.Where(a => a.ColumnNo == column).FirstOrDefault();
                   //     if (loc != null)
                   //     {
                   //         locationList1.Add(loc);
                   //     }
                   //     else
                   //     {
                   //         Location l = new Location();
                   //         l.Id = 0;

                   //         locationList1.Add(l);
                   //     }

                   // }

           

                   //var floor2=lineList.Where(a => a.FloorNo == 2).OrderBy(a => a.ColumnNo).ToList();

                   // List<Location> locationList2 = new List<Location>();

                   // for (var column = 1; column <= 46; column++)
                   // {

                   //     var loc = floor2.Where(a => a.ColumnNo == column).FirstOrDefault();
                   //     if (loc != null)
                   //     {
                   //         locationList2.Add(loc);
                   //     }
                   //     else
                   //     {
                   //         Location l = new Location();
                   //         l.Id = 0;

                   //         locationList2.Add(l);
                   //     }

                   // }

                   // result.Floor1 = locationList1;
                   // result.Floor2 = locationList2;                   
                   // result.Floor3 = lineList.Where(a => a.FloorNo == 3).OrderBy(a => a.ColumnNo).ToList();
                   // result.Floor4 = lineList.Where(a => a.FloorNo == 4).OrderBy(a => a.ColumnNo).ToList();
                   // result.Floor5 = lineList.Where(a => a.FloorNo == 5).OrderBy(a => a.ColumnNo).ToList();

                }
                
                //else if (currentWarehouseId == 4)//地板
                //{

                //    var floor1 = lineList.Where(a => a.FloorNo == 1).OrderBy(a => a.ColumnNo).ToList();
                //    List<Location> locationList1 = new List<Location>();

                //    for (var column = 1; column <= 56; column++)
                //    {

                //        var loc = floor1.Where(a => a.ColumnNo == column).FirstOrDefault();
                //        if (loc != null)
                //        {
                //            locationList1.Add(loc);
                //        }
                //        else
                //        {
                //            Location l = new Location();
                //            l.Id = 0;

                //            locationList1.Add(l);
                //        }

                //    }


                   
                //    var floor2 = lineList.Where(a => a.FloorNo == 2).OrderBy(a => a.ColumnNo).ToList();

                //    List<Location> locationList2 = new List<Location>();

                //    for (var column = 1; column <= 56; column++)
                //    {

                //        var loc = floor2.Where(a => a.ColumnNo == column).FirstOrDefault();
                //        if (loc != null)
                //        {
                //            locationList2.Add(loc);
                //        }
                //        else
                //        {
                //            Location l = new Location();
                //            l.Id = 0;

                //            locationList2.Add(l);
                //        }

                //    }

                //    result.Floor1 = locationList1;
                //    result.Floor2 = locationList2;

                //    result.Floor3 = lineList.Where(a => a.FloorNo == 3).OrderBy(a => a.ColumnNo).ToList();
                //    result.Floor4 = lineList.Where(a => a.FloorNo == 4).OrderBy(a => a.ColumnNo).ToList();
              

                //    var floor5 = lineList.Where(a => a.FloorNo == 5).OrderBy(a => a.ColumnNo).ToList();
                //    List<Location> locationList5 = new List<Location>();
                //    for (var column = 1; column <= 56; column++)
                //    {

                //        var loc = floor5.Where(a => a.ColumnNo == column).FirstOrDefault();
                //        if (loc != null)
                //        {
                //            locationList5.Add(loc);
                //        }
                //        else
                //        {
                //            Location l = new Location();
                //            l.Id = 0;

                //            locationList5.Add(l);
                //        }

                //    }
                //    result.Floor5 = locationList5;
                //}
                //else if (currentWarehouseId == 5)//轮罩
                //{

                //    result.Floor1 = lineList.Where(a => a.FloorNo == 1).OrderBy(a => a.ColumnNo).ToList();
                //    result.Floor2 = lineList.Where(a => a.FloorNo == 2).OrderBy(a => a.ColumnNo).ToList();
                //    result.Floor3 = lineList.Where(a => a.FloorNo == 3).OrderBy(a => a.ColumnNo).ToList();
                //    result.Floor4 = lineList.Where(a => a.FloorNo == 4).OrderBy(a => a.ColumnNo).ToList();
                //    result.Floor5 = lineList.Where(a => a.FloorNo == 5).OrderBy(a => a.ColumnNo).ToList();

                //}
                //else if (currentWarehouseId == 6)//侧围
                //{

                //    result.Floor1 = lineList.Where(a => a.FloorNo == 1).OrderBy(a => a.ColumnNo).ToList();

                //    var floor2 = lineList.Where(a => a.FloorNo == 2).OrderBy(a => a.ColumnNo).ToList();
                //    List<Location> locationList2 = new List<Location>();
                //    if (input.Line == 1)
                //    {
                //        for (var column = 1; column <= 23; column++)
                //        {

                //            var loc = floor2.Where(a => a.ColumnNo == column).FirstOrDefault();
                //            if (loc != null)
                //            {
                //                locationList2.Add(loc);
                //            }
                //            else
                //            {
                //                Location l = new Location();
                //                l.Id = 0;

                //                locationList2.Add(l);
                //            }

                //        }
                //    }
                //    else
                //    {
                //        for (var column = 1; column <= 46; column++)
                //        {

                //            var loc = floor2.Where(a => a.ColumnNo == column).FirstOrDefault();
                //            if (loc != null)
                //            {
                //                locationList2.Add(loc);
                //            }
                //            else
                //            {
                //                Location l = new Location();
                //                l.Id = 0;

                //                locationList2.Add(l);
                //            }

                //        }
                //    }
                //    result.Floor2 = locationList2;

                //    result.Floor3 = lineList.Where(a => a.FloorNo == 3).OrderBy(a => a.ColumnNo).ToList();
                //    result.Floor4 = lineList.Where(a => a.FloorNo == 4).OrderBy(a => a.ColumnNo).ToList();
                //    result.Floor5 = lineList.Where(a => a.FloorNo == 5).OrderBy(a => a.ColumnNo).ToList();
                //    result.Floor6 = lineList.Where(a => a.FloorNo == 6).OrderBy(a => a.ColumnNo).ToList();
                //}
            }


            return result;
        }

        public async Task<GetLocataionCountOutput> GetLocataionCount()
        {
            GetLocataionCountOutput result = new GetLocataionCountOutput();
            var query = new Specification<Location>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);

            var list = await Repository.Query(query).AsNoTracking().ToListAsync();
            result.ZongCount = list.Count();
            result.ZhanCount = list.Where(a => a.LocationStatus == LocationStatus.已使用).Count();
            result.SuoDingCount = list.Where(a => a.LocationStatus == LocationStatus.锁定).Count();
            result.KongCount = list.Where(a => a.LocationStatus == LocationStatus.未使用).Count();
            return result;
        }

        /// <summary>
        /// 提供给看板的库口数据ID
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<Getkukuoutput> GetLocationKuKouList(Getkukou input)
        {
            Getkukuoutput getkukuoutputresult = new Getkukuoutput();
            var query = new Specification<Location>(u => !u.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (input.AreaId != null)
            {
                query.CombineCritia(u => u.AreaId == input.AreaId);
            }
            if (input.LocationType != null)
            {
                //只查询库位类型是库口的
                query.CombineCritia(u => u.LocationType == LocationType.库口);
            }
            var kukouList = await Repository.Query(query).AsNoTracking().ToListAsync();
            List<int> list = new List<int>();
            foreach (var item in kukouList)
            {
                list.Add(item.Id);
                getkukuoutputresult.locationID = list;
            }
            return getkukuoutputresult;
        }


        /// <summary>
        /// 根据排查询5层数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetBoardOutput> GetLocationFloor(int? line)
        {
            GetBoardOutput result = new GetBoardOutput();

            var query = new Specification<Location>(a => !a.IsDeleted);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (line > 0)
            {
                query.CombineCritia(u => u.LineNo == line);
                var list = await Repository.Query(query).AsNoTracking().ToListAsync();
                result.Floor1 = list.Where(u => u.FloorNo == 1).OrderBy(u => u.ColumnNo).ToList();
                result.Floor2 = list.Where(u => u.FloorNo == 2).OrderBy(u => u.ColumnNo).ToList();
                result.Floor3 = list.Where(u => u.FloorNo == 3).OrderBy(u => u.ColumnNo).ToList();
                result.Floor4 = list.Where(u => u.FloorNo == 4).OrderBy(u => u.ColumnNo).ToList();
                result.Floor5 = list.Where(u => u.FloorNo == 5).OrderBy(u => u.ColumnNo).ToList();
            }
            return result;
        }



    }




}
