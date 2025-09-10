using ChangSha_Byd_NetCore8.Entities.WareHouse;
using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using ChangSha_Byd_NetCore8.OpenAuth.App.Request;

namespace ChangSha_Byd_NetCore8.App.Request
{
    public class DeleteByIdsInput
    {
        public int[] Ids { get; set; }
    }
    public class WarehouseRequest
    {
    }

    public class WarehouseDto
    {
        public Warehouse Warehouse { get; set; }

        public List<Area> DetailList { get; set; }

    }

    public class QueryWarehouseReq : PageReq
    {


    }



    public class QueryLocationReq : PageReq
    {
        public int? WarehouseId { get; set; }

        public int? AreaId { get; set; }

        public int? Status { get; set; }
        public bool? IsDisabled { get; set; }
        /// <summary>
        /// 库位类型是不是库口
        /// </summary>
        public bool? IsKukou { get; set; }
        public int? Id { get; set; }

    }
    public class GetLocationInput
    {
        public int? Id { get; set; }
        public string Code { get; set; }
    }

    public class GetMoveKongLocationInput
    {
        public int? WarehouseId { get; set; }

        public int? AreaId { get; set; }

        public int? EquipmentId { get; set; }

        public string ProductId { get; set; }

        public string Batch { get; set; }

    }

    public class InitLocationInput
    {
        public string Type { get; set; }
        public int WarehouseId { get; set; }
        public int AreaId { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        /// <summary>
        /// 第几排/行
        /// </summary>
        public int LineNo { get; set; }
        /// <summary>
        /// 有几列
        /// </summary>
        public int ColumnNo { get; set; }
        /// <summary>
        /// 有几层
        /// </summary>
        public int FloorNo { get; set; }
        /// <summary>
        /// 前身位还是后身位
        /// </summary>
        public int AroudNo { get; set; }
        /// <summary>
        /// 后身位关联的第几排
        /// </summary>
        public int? RelateLineNo { get; set; }
        public int EquipmentId { get; set; }
    }

    public class InitLocationAreaInput
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? StartColumnNo { get; set; }
        public int? EndColumnNo { get; set; }

    }

    public class GetBoardInput
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? Column { get; set; }
        public int? Line { get; set; }
    }

    public class GetBoardOutput
    {
        public List<Location> Floor1 { get; set; }
        public List<Location> Floor2 { get; set; }
        public List<Location> Floor3 { get; set; }
        public List<Location> Floor4 { get; set; }
        public List<Location> Floor5 { get; set; }
        public List<Location> Floor6 { get; set; }
        ///// <summary>
        ///// 第几列
        ///// </summary>
        //public int LocationColumnNo { get; set; }
        ///// <summary>
        ///// 第几排
        ///// </summary>
        //public int LocationLineNo { get; set; }
        ///// <summary>
        ///// 一列库存情况  0表示一列都没货，1表示有货，2表示一列都有货
        ///// </summary>
        //public string BackGroundColor { get; set; }
    }

    public class GetBoardLocationStatus
    {
        public int columnNo { get; set; }
        public string BackGroundColor { get; set; }
    }
    public class GetBoardCeInput
    {
        public int? AreaId { get; set; }
        public int? Column { get; set; }
        public int? Line { get; set; }
    }
    public class GetBoardCeOutput
    {
        public int LocationId { get; set; }
        public int FloorNo { get; set; }

        public string LocationCode { get; set; }
        public string CarTypeName { get; set; }
        public string CarTypeNum { get; set; }
        public int SetTaskType { get; set; } = 0;
        public DateTime? CreateTime { get; set; }
    }

    public class GetInventoryEntityInput
    {
        public int? LocationId { get; set; }
    }
    public class GetLocataionCountOutput
    {
        /// <summary>
        /// 可用库位
        /// </summary>
        public int KongCount { get; set; }
        /// <summary>
        /// 锁定库位
        /// </summary>
        public int SuoDingCount { get; set; }
        /// <summary>
        /// 占用库位
        /// </summary>
        public int ZhanCount { get; set; }
        /// <summary>
        /// 库位总数
        /// </summary>
        public int ZongCount { get; set; }
    }
    /// <summary>
    /// 查询库口请求参数
    /// </summary>
    public class Getkukou
    {
        public int? WarehouseId { get; set; }
        public int? AreaId { get; set; }
        public int? Column { get; set; }
        public int? Line { get; set; }
        public int? LocationType { get; set; }
    }
    public class Getkukuoutput
    {
        public List<int> locationID { get; set; }//库口ID
        //public int id { get; set; }
    }
}
