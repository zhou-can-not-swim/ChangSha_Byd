namespace ChangSha_Byd_NetCore8.Protocols.Common
{
    /// <summary>
    /// 仓库类型
    /// </summary>
    public enum WarehouseType
    {
        侧围库 = 1,
        地板库 = 2,

    }

    public enum LocationLine
    {

        A线 = 1,
        B线 = 2
    }
    /// <summary>
    /// 库位状态
    /// </summary>
    public enum LocationStatus
    {
        未使用 = 0,
        已使用 = 1,
        锁定 = 2
    }
    /// <summary>
    /// 库位类型
    /// </summary>
    public enum LocationType
    {
        正常尺寸 = 0,
        超宽或超高 = 1,
        库口 = 2,
        维修口 = 3
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskType
    {
        入库 = 1,
        出库 = 2,
        移库 = 3,
        存车修正 = 4
    }
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        初始化 = 0,
        等待执行 = 1,
        正在执行 = 2,
        任务完成 = 3,
        故障 = 4,
        暂停 = 5,
        已校验等待出库 = 6,
        校验RFID不通过 = 7,
        取消任务 = 9
    }

    /// <summary>
    ///  任务优先级
    /// </summary>
    public enum TaskPriority
    {
        普通 = 0,
        优先 = 1
    }

    /// <summary>
    /// 库口类型
    /// </summary>
    public enum GatewayType : int
    {
        初始化 = 0,
        进口 = 1,
        出口 = 2

    }





}
