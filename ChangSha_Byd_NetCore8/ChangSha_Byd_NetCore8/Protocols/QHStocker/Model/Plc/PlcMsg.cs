using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Attr;
using System.Runtime.InteropServices;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc
{
    /// <summary>
    /// PLC读取     PLC 通过 PlcMsg 向上位机反馈状态信息。
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]//3 0-7
    public class PlcMsg
    {
        public const int DB_INDEX = 13;
        public const int DB_OFFSET = 0;


        /// <summary>
        /// 相互确认的信息,放在了本文件中
        /// </summary>
        public PlcFlags_GeneralCmdWord GeneralCmdWord;

        public byte 预留1;

        /// <summary>
        /// 放在了CommonEnum.cs中
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public StockerStatus 堆垛机状态;

        [Endian(Endianness.BigEndian)]
        public StockerTrip 行程;

        [Endian(Endianness.BigEndian)]
        public StockerCargo 堆垛机是否载货;

        [Endian(Endianness.BigEndian)]
        public StockerAction 动作;

        [Endian(Endianness.BigEndian)]
        public ushort 堆垛机当前层号;

        [Endian(Endianness.BigEndian)]
        public ushort 堆垛机当前排号;

        [Endian(Endianness.BigEndian)]
        public ushort 堆垛机当前列号;

        [Endian(Endianness.BigEndian)]
        public ushort 执行包号;

        [Endian(Endianness.BigEndian)]
        public ushort 完成包号;

        [Endian(Endianness.BigEndian)]
        public ushort 下发任务校验结果;

        /// <summary>
        /// 放在本文件中
        /// </summary>
        public AlarmError 报警1;
        public AlarmError 报警2;
        public AlarmError 报警3;
        public AlarmError 报警4;
        public AlarmError 报警5;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 68)]
        public byte[] 预留2;


        public PlcMsg_Gateway EC010_A库口; [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)] public byte[] 预留EC010_A库口;
        public PlcMsg_Gateway EC010_B库口; [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)] public byte[] 预留EC010_B库口;


    }

    /// <summary>
    /// 封装一个PlsMsg
    /// 对前100的指令的封装
    /// 对一个库口的的实例化
    /// </summary>
    public class PlcInfo
    {
        private readonly PlcMsg msg;

        
        internal PlcInfo(PlcMsg msg)
        {
            this.msg = msg;
        }

        public bool heartBeatAck => msg.GeneralCmdWord.HasFlag(PlcFlags_GeneralCmdWord.心跳响应);

        public bool heartBeatReq => msg.GeneralCmdWord.HasFlag(PlcFlags_GeneralCmdWord.心跳请求);

        public bool SendTaskAck => msg.GeneralCmdWord.HasFlag(PlcFlags_GeneralCmdWord.下发任务确认);

        public bool FinishTaskReq => msg.GeneralCmdWord.HasFlag(PlcFlags_GeneralCmdWord.完成任务请求);
        public bool RequestTaskAck => msg.GeneralCmdWord.HasFlag(PlcFlags_GeneralCmdWord.下发RFID校验确认);
        public bool RequestTaskResultReq => msg.GeneralCmdWord.HasFlag(PlcFlags_GeneralCmdWord.RFID校验结果请求);
        public bool RequestOutTaskAck => msg.GeneralCmdWord.HasFlag(PlcFlags_GeneralCmdWord.请求出库确认);


        public StockerStatus StorkerStatus => msg.堆垛机状态;
        public StockerTrip StockerTrip => msg.行程;
        public StockerCargo StockerCargo => msg.堆垛机是否载货;
        public StockerAction StockerAction => msg.动作;
        public int CurrentFloor => (int)msg.堆垛机当前层号;
        public int CurrentLine => (int)msg.堆垛机当前排号;
        public int CurrentColumn => (int)msg.堆垛机当前列号;
        public int DoTaskNo => (int)msg.执行包号;
        public int TaskNo => (int)msg.完成包号;
        public int VerificationCode => (int)msg.下发任务校验结果;

        public string ErrorCode1 => msg.报警1.AlarmErrors(typeof(Line5EnumAlarmError1));
        public string ErrorCode2 => msg.报警2.AlarmErrors(typeof(Line5EnumAlarmError2));
        public string ErrorCode3 => msg.报警3.AlarmErrors(typeof(Line5EnumAlarmError3));
        public string ErrorCode4 => msg.报警4.AlarmErrors(typeof(Line5EnumAlarmError4));
        public string ErrorCode5 => msg.报警5.AlarmErrors(typeof(Line5EnumAlarmError5));


        /// <summary>
        /// 每一次访问库口，都会实例化
        /// </summary>
        public PlcInfo_Gateway EC010_A库口 => new PlcInfo_Gateway(msg.EC010_A库口);
        public PlcInfo_Gateway EC010_B库口 => new PlcInfo_Gateway(msg.EC010_B库口);



    }

    /// <summary>
    /// 相互确认的信息
    /// </summary>
    [Flags]
    public enum PlcFlags_GeneralCmdWord : byte
    {
        None = 0,
        心跳响应 = 1 << 0,
        心跳请求 = 1 << 1,
        下发任务确认 = 1 << 2,
        完成任务请求 = 1 << 3,
        下发RFID校验确认 = 1 << 4,
        RFID校验结果请求 = 1 << 5,
        请求出库确认 = 1 << 6

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class AlarmError
    {
        [Endian(Endianness.BigEndian)]
        public short Content;

        public string AlarmErrors(Type enumType)
        {
            string result = "";
            var str = Convert.ToString(Content, 2);
            var charArray = str.ToCharArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                var value = charArray[charArray.Length - (i + 1)] == '1';
                if (value)
                {
                    if (enumType == typeof(EnumAlarmError5))
                    {
                        var enumI = Enum.ToObject(enumType, (i + 1) + 8);
                        result += enumI.ToString() + ";";
                    }
                    else
                    {
                        var enumI = Enum.ToObject(enumType, (i + 1));
                        result += enumI.ToString() + ";";
                    }

                }
            }

            return result;
        }

    }
}
