using ChangSha_Byd_NetCore8.Entities.WarehouseModel;
using FutureTech.Dal.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities.WareHouse
{
    /// <summary>
    /// 车型
    /// </summary>
    [Table("CarType")]
    public class CarType : FutureBaseEntity<int>
    {
        /// <summary>
        /// 车型编码
        /// </summary>
        public string Code { get; set; }
        ///// <summary>
        ///// 类型   SC2E的为1，EK的为2
        ///// </summary>
        //public int Num { get; set; }
        /// <summary>
        /// 车型名称
        /// </summary>
        public string Name { get; set; }
        public int MinRFID { get; set; }
        public int MaxRFID { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

      }
}
