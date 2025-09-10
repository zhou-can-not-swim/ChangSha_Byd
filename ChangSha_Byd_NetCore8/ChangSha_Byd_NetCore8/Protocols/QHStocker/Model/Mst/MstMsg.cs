using ChangSha_Byd_NetCore8.Protocols.Common;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Attr;
using System.Runtime.InteropServices;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst
{
    /// <summary>
    /// MST 写入plc  上位机通过 MstMsg 向 PLC 发送控制指令。
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg
    {
        public MstMsg(MstMsg msg)
        {
            this.GeneralCmdWord = msg.GeneralCmdWord;
            this.预留1 = msg.预留1;
            this.Core = msg.Core;
            this.预留2 = msg.预留2;
            this.GateWay = msg.GateWay;
        }
        public MstMsg() { }

        public const int DB_INDEX = 11;
        public const int DB_OFFSET = 0;

        /// <summary>
        /// 相互确认的信息
        /// </summary>
        public MstFlags_GeneralCmdWord GeneralCmdWord;

        public byte 预留1;

        /// <summary>
        /// 下任务内容
        /// </summary>
        public MstMsg_Core Core;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 76)]
        public byte[] 预留2;

        /// <summary>
        /// 所有库口信息
        /// </summary>
        public MstMsgGateWay GateWay;

    }

    public class MstInfo
    {
        private readonly MstMsg msg;

        /// 封装一个PlsMsg
        internal MstInfo(MstMsg msg)
        {
            this.msg = msg;
        }

        public bool heartBeatReq => msg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.心跳请求);
        public bool heartBeatAck => msg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.心跳响应);

        public bool SendTaskReq => msg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.下发任务请求);

        public bool FinishTaskAck => msg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.完成任务确认);
        public bool RequsetTaskReq => msg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.下发RFID校验请求);
        public bool RequsetTaskResultAck => msg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.RFID校验结果确认);
        public bool RequsetOutTaskReq => msg.GeneralCmdWord.HasFlag(MstFlags_GeneralCmdWord.请求出库);


        /// <summary>
        /// 任务号
        /// </summary>
        public int TaskNo => (int)msg.Core.TaskNo;
        /// <summary>
        /// 命令代码
        /// </summary>
        public PLCTaskType TaskType => msg.Core.TaskType;

        /// <summary>
        /// 源排号
        /// </summary>
        public int StartLine => (int)msg.Core.StartLine;
        /// <summary>
        /// 源层号
        /// </summary>
        public int StartFloor => (int)msg.Core.StartFloor;
        /// <summary>
        /// 源列号
        /// </summary>
        public int StartColumn => (int)msg.Core.StartColumn;

        /// <summary>
        /// 目的排号
        /// </summary>
        public int EndLine => (int)msg.Core.EndLine;
        /// <summary>
        /// 目的层号
        /// </summary>
        public int EndFloor => (int)msg.Core.EndFloor;
        /// <summary>
        /// 目的列号
        /// </summary>
        public int EndColumn => (int)msg.Core.EndColumn;
        /// <summary>
        /// 校验号
        /// </summary>
        public int VerificationCode => (int)msg.Core.VerificationCode;

        /// <summary>
        /// 下发任务RIFD
        /// </summary>
        public short TaskRFID => msg.Core.TaskRFID;

        public MstInfo_Gateway EC010_A库口 => new MstInfo_Gateway(msg.GateWay.EC010_A库口);
        public MstInfo_Gateway EC010_B库口 => new MstInfo_Gateway(msg.GateWay.EC010_B库口);


    }
   

    [Flags]
    public enum MstFlags_GeneralCmdWord : byte
    {
        None = 0,
        心跳请求 = 1 << 0,
        心跳响应 = 1 << 1,
        下发任务请求 = 1 << 2,
        完成任务确认 = 1 << 3,
        下发RFID校验请求 = 1 << 4,
        RFID校验结果确认 = 1 << 5,
        请求出库 = 1 << 6,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_Core
    {

        public MstMsg_Core(MstMsg_Core core)
        {
            this.TaskNo = core.TaskNo;
            this.TaskType = core.TaskType;
            this.StartLine = core.StartLine;
            this.StartFloor = core.StartFloor;
            this.StartColumn = core.StartColumn;
            this.EndLine = core.EndLine;
            this.EndFloor = core.EndFloor;
            this.EndColumn = core.EndColumn;
            this.VerificationCode = core.VerificationCode;
            this.TaskRFID = core.TaskRFID;
        }

        /// <summary>
        /// 下发任务包号 2 -- 22
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public ushort TaskNo;
        /// <summary>
        /// 命令代码
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public PLCTaskType TaskType;

        /// <summary>
        /// 源排号
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public ushort StartLine;
        /// <summary>
        /// 源层号
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public ushort StartFloor;
        /// <summary>
        /// 源列号
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public ushort StartColumn;

        /// <summary>
        /// 目的排号
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public ushort EndLine;
        /// <summary>
        /// 目的层号
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public ushort EndFloor;
        /// <summary>
        /// 目的列号
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public ushort EndColumn;
        /// <summary>
        /// 校验号
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public int VerificationCode;
        /// <summary>
        ///下发任务rfid 
        /// </summary>
        [Endian(Endianness.BigEndian)]
        public short TaskRFID;



    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsgGateWay
    {
        public MstMsg_GateWay EC010_A库口; [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)] public byte[] 预留EC010_A库口;
        public MstMsg_GateWay EC010_B库口; [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)] public byte[] 预留EC010_B库口;

    }

}
