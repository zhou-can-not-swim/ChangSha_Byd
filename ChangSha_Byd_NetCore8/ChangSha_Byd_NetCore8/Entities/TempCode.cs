using FutureTech.Dal.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangSha_Byd_NetCore8.Entities
{
    /// <summary>
    /// 临时变量
    /// </summary>
    [Table("TempCode")]
    public class TempCode : FutureBaseEntity<int>
    {
        public int Code { get; set; }
        public string StrCode { get; set; }
        public int WarehouseId { get; set; }

        
    }
}
