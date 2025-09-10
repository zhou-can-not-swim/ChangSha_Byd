using AsZero.DbContexts;
using Byd.Services.Request;
using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChangSha_Byd_NetCore8.App.WarehouseModel
{

    public class CarTypeApp : FutureBaseEntityService<int, CarType>
    {
        public IGenericRepository<int, CarType> _carTypeRespository { get; }
        private readonly IOptions<AppSetting> _appConfiguration;
        public readonly AsZeroDbContext _dBContext;
        public CarTypeApp(IGenericRepository<int, CarType> repo, IOptions<AppSetting> appConfiguration, AsZeroDbContext dBContext) : base(repo)
        {
            _carTypeRespository = repo;
            _appConfiguration = appConfiguration;
            _dBContext = dBContext;
        }
        /// <returns></returns>
        public async Task<TableData> Load(QueryCarTypeReq request)
        {

            var query = new Specification<CarType>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.Code.Contains(request.key) || u.Name.Contains(request.key));
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);

            var (count, data) = await LoadPageAsNoTrackingAsync(query, request.page, request.limit);

            return new TableData { count = count, data = data };
        }
        public async Task<IReadOnlyList<CarType>> GetList(QueryCarTypeReq input)
        {

            var query = new Specification<CarType>(u => !u.IsDeleted);
            if (!string.IsNullOrEmpty(input.key))
            {
                query.CombineCritia(u => u.Code.Contains(input.key) || u.Name.Contains(input.key));
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var list = await Repository.Query(query).AsNoTracking().ToListAsync();

            return list;
        }

        public async Task<bool> IsHaveType(CarType obj)
        {
            var query = new Specification<CarType>(a => !a.IsDeleted && a.Code == obj.Code && a.MinRFID == obj.MinRFID && a.MaxRFID == obj.MaxRFID);
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            if (obj.Id > 0)
            {
                query.CombineCritia(u => u.Id != obj.Id);
            }

            var i = await Repository.Query(query).AsNoTracking().CountAsync();
            if (i > 0) { return true; }
            else { return false; }
        }

        /// <summary>
        /// 获取车型对象
        /// </summary>
        /// <param name="inpu"></param>
        /// <returns></returns>
        public async Task<CarType> GetCarTypeEntity(GetCarTypeEntityInput input)
        {
            var query = new Specification<CarType>(u => !u.IsDeleted);
            if (!string.IsNullOrEmpty(input.CarTypeCode))
            {
                query.CombineCritia(u => u.Code == input.CarTypeCode);
            }
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            return await Repository.Query(query).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// 通过台车编号获得车型类型
        /// </summary>
        /// <param name="CarTypeNum">台车编号 2041</param>
        /// <param name="GateWayId">入库口id 10</param>
        /// <returns></returns>
        public async Task<CarType> GetCarTypeByCarTypeNum(string CarTypeNum, int GateWayId)
        {
            CarType result = null;
            var currentWarehouseId = _appConfiguration.Value.WarehouseId;
            var query = new Specification<CarType>(u => !u.IsDeleted);
            query.CombineCritia(u => u.WarehouseId == currentWarehouseId);
            var list = await Repository.Query(query).AsNoTracking().ToListAsync();

            //抓取当字符串中数字部分
            int result_shuzi = int.Parse(System.Text.RegularExpressions.Regex.Replace(CarTypeNum, @"[^0-9]+", ""));
            //抓取当字符串中字符部分
            string result_zifu = System.Text.RegularExpressions.Regex.Replace(CarTypeNum, @"\d", "");

            if (currentWarehouseId == 1)//侧围轮罩库
            {
                var rfid = int.Parse(result_shuzi.ToString().Substring(0, 1) + "0" + result_shuzi.ToString().Substring(2, 2));

                result = list.Where(a => a.MinRFID <= rfid && rfid <= a.MaxRFID).FirstOrDefault();

            }
            else if ( currentWarehouseId ==2  ) //地板库
            {
                var rfid = int.Parse(result_shuzi.ToString().Substring(0, 1) + "0" + result_shuzi.ToString().Substring(2, 2));

                result = list.Where(a => a.MinRFID <= rfid && rfid <= a.MaxRFID).FirstOrDefault();
            }




                //  var isZifu = System.Text.RegularExpressions.Regex.Replace(rfidFirst, @"\d", "");

                //var rfidFirst = CarTypeNum.Substring(0, 2);
                //var rfidNumcode = int.Parse(CarTypeNum.Substring(2, CarTypeNum.Length));//取出台车号
                //result = list.Where(a => a.Code == rfidFirst && a.MinRFID <= rfidNumcode && rfidNumcode <= a.MaxRFID).FirstOrDefault();



                //如果传输的台车号16进制，转换完成在a对比
                //var rfidNumcodeString = CarTypeNum.Substring(2, CarTypeNum.Length-2);
                //var rfidNumcode = Convert.ToInt32(rfidNumcodeString, 16);
                //result = list.Where(a => a.Code == rfidFirst && a.MinRFID <= rfidNumcode && rfidNumcode <= a.MaxRFID).FirstOrDefault();
            //}
            //else
            //{
            //    //只针对16进制1-9款车型,如果第10种以上车型校验无法通过校验
            //    if (result_shuzi.ToString().Length == 4)
            //    {
            //        if (currentWarehouseId == 3)//盖库
            //        {
            //            var gateway = await _dBContext.Gateways.Where(a => !a.IsDeleted && a.Id == GateWayId).AsNoTracking().FirstOrDefaultAsync();
            //            if (gateway.Remark == "后盖内板胎膜")
            //            {
            //                var rfid = int.Parse(result_shuzi.ToString());
            //                result = list.Where(a => a.MinRFID <= rfid && rfid <= a.MaxRFID).FirstOrDefault();
            //            }
            //            else
            //            {
            //                var rfid = int.Parse(result_shuzi.ToString().Substring(0, 1) + "0" + result_shuzi.ToString().Substring(2, 2));

            //                result = list.Where(a => a.MinRFID <= rfid && rfid <= a.MaxRFID).FirstOrDefault();
            //            }
            //        }
            //        else if (currentWarehouseId == 5)
            //        {
            //            var rfid = int.Parse(result_shuzi.ToString().Substring(0, 1) + "0" + result_shuzi.ToString().Substring(2, 2));

            //            result = list.Where(a => a.MinRFID <= rfid && rfid <= a.MaxRFID).FirstOrDefault();
            //        }
            //        else if (currentWarehouseId == 4)
            //        {//地板库
                        
            //            var rfid = int.Parse(result_shuzi.ToString().Substring(0, 1) + "0" + result_shuzi.ToString().Substring(2, 2));

            //            result = list.Where(a => a.MinRFID <= rfid && rfid <= a.MaxRFID).FirstOrDefault();

            //        }
            //        else
            //        {
            //            var rfid = int.Parse(result_shuzi.ToString().Substring(0, 1) + "0" + result_shuzi.ToString().Substring(2, 2));
            //            result = list.Where(a => a.MinRFID <= rfid && rfid <= a.MaxRFID).FirstOrDefault();
            //            // result = list.Where(a => a.MinRFID <= result_shuzi && result_shuzi <= a.MaxRFID).FirstOrDefault();
            //        }
            //    }else if (result_shuzi.ToString().Length == 3 && !string.IsNullOrEmpty(result_zifu))
            //    {
            //        var rfid = int.Parse(result_shuzi.ToString().Substring(0, 1) + "0" + result_shuzi.ToString().Substring(1, 2));
            //        result = list.Where(a => a.MinRFID <= rfid && rfid <= a.MaxRFID).FirstOrDefault();


            //    }






            //}


            return result;


        }

    }
}
