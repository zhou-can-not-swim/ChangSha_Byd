namespace ChangSha_Byd_NetCore8.Extends.Scan
{
    public class StationOptionItem
    {
        public bool Enabled { get; set; }//必要的，在中间件扫描开始的时候会检查一次，确认机器已经开了
    }

    public class ScanOpts
    {
        public ScanOpts()
        {
            QH_堆垛机 = new StationOptionItem();
            侧围_堆垛机 = new StationOptionItem();
        }

        public StationOptionItem QH_堆垛机 { get; set; }
        public StationOptionItem 侧围_堆垛机 { get; set; }
    }
}
