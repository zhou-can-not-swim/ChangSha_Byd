namespace ChangSha_Byd_NetCore8.Extends.PlcServices
{
    /// <summary>
    /// PLC连接参数
    /// </summary>
    public class S7PlcOptItem
    {
        public string IpAddr { get; set; } = "127.0.0.1";

        public short Rack { get; set; }

        public short Slot { get; set; } = 1;

    }
}
