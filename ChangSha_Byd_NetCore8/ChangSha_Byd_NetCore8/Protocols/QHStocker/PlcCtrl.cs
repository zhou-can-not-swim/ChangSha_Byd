using ChangSha_Byd_NetCore8.Extends;
using ChangSha_Byd_NetCore8.Extends.PlcServices;
using ChangSha_Byd_NetCore8.fan;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Mst;
using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Plc;
using ChangSha_Byd_NetCore8.Utils;
using Microsoft.Extensions.Options;
using Microsoft.FSharp.Core;
using System.Runtime.InteropServices;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker
{
    /// <summary>
    /// PLC控制器,包括读写操作，最下层还有s7通信
    /// 是对s7的封装
    /// </summary>
    public class PlcCtrl : S7PlcCtrl
    {
        public PlcCtrl(string plcName, IOptionsMonitor<S7PlcOptItem> optionsMonitor) : base(plcName, optionsMonitor)
        {
        }


        /// <summary>
        /// 读取DEV下位机
        /// </summary>
        /// <returns></returns>
        public async Task<FSharpResult<PlcMsg, string>> ReadPlcMsgAsync()
        {                                  //13
            var bytes = await ReadDBAsync(PlcMsg.DB_INDEX, PlcMsg.DB_OFFSET, Marshal.SizeOf<PlcMsg>());
            if (bytes.IsError)
            {
                return FSharpResult<PlcMsg, string>.NewError(bytes.ErrorValue.ToString());
            }
            var bytesn = bytes.ResultValue;
            var plcmsg = MarshalHelper.BytesToStruct<PlcMsg>(bytesn);//test有值了
            return FSharpResult<PlcMsg, string>.NewOk(plcmsg);
        }

        /// <summary>
        /// 读取MST 上位机   （一种消息结构？）
        /// </summary>
        /// <returns></returns>
        public async Task<FSharpResult<MstMsg, string>> ReadMstMsgAsync()
        {
            //从PLC读取MST消息。                    //从11db块        偏移量          字节数
            var bytes = await this.ReadDBAsync(MstMsg.DB_INDEX, MstMsg.DB_OFFSET, Marshal.SizeOf<MstMsg>());
            if (bytes.IsError)
            {
                return FSharpResult<MstMsg, string>.NewError(bytes.ErrorValue.ToString());
            }
            var bytesn = bytes.ResultValue;
            //将字节数组转换为MstMsg结构体
            var mstmsg = MarshalHelper.BytesToStruct<MstMsg>(bytesn);
            return FSharpResult<MstMsg, string>.NewOk(mstmsg);
        }


        /// <summary>
        /// 发送控制指令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public async Task<FSharpResult<bool, string>> SendCmdAsync(MstMsg cmd)
        {
            var bytes = MarshalHelper.StructToBytes(cmd);
            FSharpResult<bool, string> res = await this.WriteDBAsync(MstMsg.DB_INDEX, MstMsg.DB_OFFSET, bytes);
            return res;
        }


    }
}
