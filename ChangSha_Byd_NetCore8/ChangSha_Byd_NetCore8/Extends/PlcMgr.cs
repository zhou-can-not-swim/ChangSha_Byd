using ChangSha_Byd_NetCore8.Protocols.QHStocker;

namespace ChangSha_Byd_NetCore8.Extends
{
    /// <summary>
    /// PLC管理
    /// </summary>
    public class PlcMgr
    {
        public PlcMgr(IServiceProvider sp)
        {
            PlcName_QHStocker = ActivatorUtilities.CreateInstance<PlcCtrl>(sp, PlcNames.PlcName_QHStocker);
            //PlcName_CWStocker = ActivatorUtilities.CreateInstance<CWStocker.PlcCtrl>(sp, PlcNames.PlcName_CWStocker);
        }

        //去使用s7
        public PlcCtrl PlcName_QHStocker { get; }
        //public CWStocker.PlcCtrl PlcName_CWStocker { get; }
    }

    /// <summary>
    /// PLC设备名定义
    /// </summary>
    public class PlcNames
    {
        public const string PlcName_QHStocker = "QH_堆垛机";
        public const string PlcName_CWStocker = "CW_堆垛机";
    }
}
