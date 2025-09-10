using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model
{
    //管理自动化仓储系统中的标志位（flags）和位置信息
    public class MstFlagsGeneralBuilder
    {
        //上位机 在页面可能看到
        private MstFlags_GeneralCmdWord _mstFlags;

        public MstFlagsGeneralBuilder(MstFlags_GeneralCmdWord mstFlags)
        {
            this._mstFlags = mstFlags;
        }

        /// <summary>
        /// helper
        /// </summary>
        /// <param name="bitIndicator"></param>
        /// <param name="onoff">ture的时候置1，false置0</param>
        /// <returns></returns>
        /// 
        //对标志位进行 01操作
        private MstFlagsGeneralBuilder SetOnOff(MstFlags_GeneralCmdWord bitIndicator, bool onoff)
        {
            this._mstFlags = onoff ?
                this._mstFlags | bitIndicator :
                this._mstFlags & ~bitIndicator;
            return this;
        }

        /// <summary>
        /// 构建上料标志
        /// </summary>
        /// <returns></returns>
        public MstFlags_GeneralCmdWord Build() => this._mstFlags;


        public MstFlagsGeneralBuilder 下发任务请求(bool onff)
        {
            this.SetOnOff(MstFlags_GeneralCmdWord.下发任务请求, onff);
            return this;
        }
        public MstFlagsGeneralBuilder 完成任务确认(bool onff)
        {
            this.SetOnOff(MstFlags_GeneralCmdWord.完成任务确认, onff);
            return this;
        }
        public MstFlagsGeneralBuilder 下发RFID校验请求(bool onff)
        {
            this.SetOnOff(MstFlags_GeneralCmdWord.下发RFID校验请求, onff);
            return this;
        }
        public MstFlagsGeneralBuilder RFID校验结果确认(bool onff)
        {
            this.SetOnOff(MstFlags_GeneralCmdWord.RFID校验结果确认, onff);
            return this;
        }

        public MstFlagsGeneralBuilder 下发请求出库(bool onff)
        {
            this.SetOnOff(MstFlags_GeneralCmdWord.请求出库, onff);
            return this;
        }

        /// <summary>
        /// 设置心跳确认位开/关
        /// </summary>
        /// <param name="onoff"></param>
        /// <returns></returns>
        public MstFlagsGeneralBuilder 响应PLC心跳请求(bool onoff) => this.SetOnOff(MstFlags_GeneralCmdWord.心跳响应, onoff);
        /// <summary>
        /// 设置心跳请求位开/关
        /// </summary>
        /// <param name="onoff"></param>
        /// <returns></returns>
        public MstFlagsGeneralBuilder 下发心跳请求(bool onoff) => this.SetOnOff(MstFlags_GeneralCmdWord.心跳请求, onoff);

    }

    /// <summary>
    /// 11 块 100.0库口信号的操作 入口就位确认
    /// </summary>
    public class MstFlagsGatewayBuilder
    {
        private MstMsg_GatewayFlags _mstGateway;

        public MstFlagsGatewayBuilder(MstMsg_GatewayFlags mstGateway)
        {
            this._mstGateway = mstGateway;
        }
        /// <summary>
        /// 构建命令字
        /// </summary>
        /// <returns></returns>
        public MstMsg_GatewayFlags Build() => this._mstGateway;

        /// <summary>
        /// helper
        /// </summary>
        /// <param name="bitIndicator"></param>
        /// <param name="onoff">ture的时候置1，false置0</param>
        /// <returns></returns>
        private MstFlagsGatewayBuilder SetOnOff(MstMsg_GatewayFlags bitIndicator, bool onoff)
        {
            this._mstGateway = onoff ?
                this._mstGateway | bitIndicator :
                this._mstGateway & ~bitIndicator;
            return this;
        }

        public MstFlagsGatewayBuilder 入口就位确认(bool onoff)
        {

            this.SetOnOff(MstMsg_GatewayFlags.就位确认, onoff);
            return this;
        }

    }

    /// <summary>
    /// QH入库口,也就是AB工位的那个玩意
    /// </summary>
    public enum QH_EntryLocation
    {
        EC010_A工位入口 = 1,
        EC010_B工位入口 = 2,
        EC020_A工位入口 = 3,
        EC020_B工位入口 = 4,
        EC030_A工位入口 = 5,
        EC030_B工位入口 = 6,
        EC040_A工位入口 = 7,
        EC040_B工位入口 = 8,
        RF040_B工位入口 = 9,
        RF040_A工位入口 = 10,
        RF030_B工位入口 = 11,
        RF030_A工位入口 = 12,
        RF010工位入口 = 13,
        FF010_A工位入口 = 14,
        FF010_B工位入口 = 15,
        FF020_A工位入口 = 16,
        FF020_B工位入口 = 17,
        FF030_A工位入口 = 18,
        FF030_B工位入口 = 19,
        FF040_A工位入口 = 20,
        FF040_B工位入口 = 21,
        RSM010_A工位入口 = 22,
        RSM010_B工位入口 = 23,
        RSM020_A工位入口 = 24,
        RSM020_B工位入口 = 25,
        RSM030_A工位入口 = 26,
        RSM030_B工位入口 = 27,
        RSM040_A工位入口 = 28,
        RSM040_B工位入口 = 29,
        RSM050_A工位入口 = 30,
        RSM050_B工位入口 = 31,

    }

    /// <summary>
    /// 地板库出库口
    /// </summary>
    public enum QH_OutLocation
    {
        EC010_A工位出口 = 32,
        EC010_B工位出口 = 33,
        EC020_A工位出口 = 34,
        EC020_B工位出口 = 35,
        EC030_A工位出口 = 36,
        EC030_B工位出口 = 37,
        EC040_A工位出口 = 38,
        EC040_B工位出口 = 39,
        RF040_B工位出口 = 40,
        RF040_A工位出口 = 41,
        RF030_B工位出口 = 42,
        RF030_A工位出口 = 43,
        RF010工位出口 = 44,
        FF010_A工位出口 = 45,
        FF010_B工位出口 = 46,
        FF020_A工位出口 = 47,
        FF020_B工位出口 = 48,
        FF030_A工位出口 = 49,
        FF030_B工位出口 = 50,
        FF040_A工位出口 = 51,
        FF040_B工位出口 = 52,
        RSM010_A工位出口 = 53,
        RSM010_B工位出口 = 54,
        RSM020_A工位出口 = 55,
        RSM020_B工位出口 = 56,
        RSM030_A工位出口 = 57,
        RSM030_B工位出口 = 58,
        RSM040_A工位出口 = 59,
        RSM040_B工位出口 = 60,
        RSM050_A工位出口 = 61,
        RSM050_B工位出口 = 62,

    }
}
