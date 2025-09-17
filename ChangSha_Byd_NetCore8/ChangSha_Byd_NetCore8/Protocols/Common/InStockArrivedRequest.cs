using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.Common
{
    //表明InStockArrivedRequest是需要 处理类 处理的对象
    public class InStockArrivedRequest : IRequest<InStockArrivedResponse>
    {
        /// <summary>
        /// 库口入口id
        /// </summary>
        public int InStockNo { get; set; }

        public string Rfid16 { get; set; }//夹具库此时作为16进制使用

        public string Remark { get; set; } = "";//

        public string RFIDTen { get; set; } = "";//夹具库新增10进制    
    }

    public class InStockArrivedResponse
    {
        public bool IsSucess { get; set; }

        public string Msg { get; set; }

        public Object Result { get; set; }


    }
}
