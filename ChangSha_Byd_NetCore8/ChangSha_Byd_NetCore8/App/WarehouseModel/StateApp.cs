using AsZero.DbContexts;
using Byd.Services.Request;
using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using ChangSha_Byd_NetCore8.OpenAuth.App.Response;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using Microsoft.Extensions.Options;

namespace ChangSha_Byd_NetCore8.App.WarehouseModel
{
    public class StateApp : FutureBaseEntityService<int, State>
    {
        public readonly AsZeroDbContext _dBContext;
        private readonly IOptions<AppSetting> _appConfiguration;
        public StateApp(IGenericRepository<int, State> repo, AsZeroDbContext dBContext, IOptions<AppSetting> appConfiguration) : base(repo)
        {
            _dBContext = dBContext;
            _appConfiguration = appConfiguration;
        }
        public async Task<TableData> Load(QueryStateReq request)
        {

            var query = new Specification<State>(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(request.key))
            {
                query.CombineCritia(u => u.EquipmentName.Contains(request.key) || u.Dname.Contains(request.key));
            }
            var (count, data) = await LoadPageAsNoTrackingAsync(query, request.page, request.limit);

            return new TableData { count = count, data = data };
        }



        /// <summary>
        /// 插入异常信息数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<State> AddInState(AddInStateTaskInput input)
        {
            using (var transaction = _dBContext.Database.BeginTransaction())
            {
                try
                {
                    State states= new State();
                    states.WarehouseId = input.WarehouseId;
                    states.WarehouseName = input.WarehouseName;
                    states.EquipmentId = input.EquipmentId;
                    states.EquipmentName = input.EquipmentName;
                    states.Dname = input.Dname;
                    states.Dtrip=input.Dtrip;
                    states.Conent=input.Conent;
                    states.TaskTime = DateTime.Now;
                    var res = await Repository.AddAsync(states, false);
                    await Repository.SaveChangesAsync();
                    transaction.Commit();
                    return res;
                   
                }
                catch (Exception ex)
                {
                    string a = ex.Message;
                    transaction.Rollback();
                    return null;
                }
            }
        }
    }
}
