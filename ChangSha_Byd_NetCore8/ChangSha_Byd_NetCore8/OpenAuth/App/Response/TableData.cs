using ChangSha_Byd_NetCore8.OpenAuth.Infra;

namespace ChangSha_Byd_NetCore8.OpenAuth.App.Response
{
    /// <summary>
    /// table的返回数据
    /// </summary>
    public class TableData
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 操作消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 总记录条数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        ///  返回的列表头信息
        /// </summary>
        public List<KeyDescription> columnHeaders;

        /// <summary>
        /// 数据内容
        /// </summary>
        public dynamic data { get; set; }

        public TableData()
        {
            code = 200;
            msg = "加载成功";
            columnHeaders = new List<KeyDescription>();
        }
    }
}
