namespace ChangSha_Byd_NetCore8.fan
{
    public static class S7ErrorCodeHelper
    {
        public static ApiErrorCode InterpretErrorCode(int code)
        {
            short osSocketError = (short)(code & 0xFFFF);
            short num = (short)(code >> 16);
            byte isoTcpError = (byte)((uint)num & 0xFu);
            short s7Error = (short)(num >> 4);
            ApiErrorCode result = default(ApiErrorCode);
            result.OsSocketError = osSocketError;
            result.IsoTcpError = (IsoTcpErrors)isoTcpError;
            result.S7Error = s7Error;
            return result;
        }

        public static string ErrorCodeToText(int code)
        {
            return code switch
            {
                0 => "OK",
                1 => "SYS : Error creating the Socket",
                2 => "TCP : Connection Timeout",
                3 => "TCP : Connection Error",
                4 => "TCP : Data receive Timeout",
                5 => "TCP : Error receiving Data",
                6 => "TCP : Data send Timeout",
                7 => "TCP : Error sending Data",
                8 => "TCP : Connection reset by the Peer",
                9 => "CLI : Client not connected",
                10065 => "TCP : Unreachable host",
                65536 => "ISO : Connection Error",
                196608 => "ISO : Invalid PDU received",
                262144 => "ISO : Invalid Buffer passed to Send/Receive",
                1048576 => "CLI : Error in PDU negotiation",
                2097152 => "CLI : invalid param(s) supplied",
                3145728 => "CLI : Job pending",
                4194304 => "CLI : too may items (>20) in multi read/write",
                5242880 => "CLI : invalid WordLength",
                6291456 => "CLI : Partial data written",
                7340032 => "CPU : total data exceeds the PDU size",
                8388608 => "CLI : invalid CPU answer",
                9437184 => "CPU : Address out of range",
                10485760 => "CPU : Invalid Transport size",
                11534336 => "CPU : Data size mismatch",
                12582912 => "CPU : Item not available",
                13631488 => "CPU : Invalid value supplied",
                14680064 => "CPU : Cannot start PLC",
                15728640 => "CPU : PLC already RUN",
                16777216 => "CPU : Cannot stop PLC",
                17825792 => "CPU : Cannot copy RAM to ROM",
                18874368 => "CPU : Cannot compress",
                19922944 => "CPU : PLC already STOP",
                20971520 => "CPU : Function not available",
                22020096 => "CPU : Upload sequence failed",
                23068672 => "CLI : Invalid data size received",
                24117248 => "CLI : Invalid block type",
                25165824 => "CLI : Invalid block number",
                26214400 => "CLI : Invalid block size",
                30408704 => "CPU : Function not authorized for current protection level",
                31457280 => "CPU : Invalid password",
                32505856 => "CPU : No password to set or clear",
                33554432 => "CLI : Job Timeout",
                36700160 => "CLI : function refused by CPU (Unknown error)",
                34603008 => "CLI : Partial data read",
                35651584 => "CLI : The buffer supplied is too small to accomplish the operation",
                37748736 => "CLI : Cannot perform (destroying)",
                38797312 => "CLI : Invalid Param Number",
                39845888 => "CLI : Cannot change this param now",
                40894464 => "CLI : Function not implemented",
                _ => "CLI : Unknown error (0x" + Convert.ToString(code, 16) + ")",
            };
        }

        public static ApiError GenerateApiError(string devName, int code)
        {
            ApiError result = default(ApiError);
            result.DevName = devName;
            result.Error = InterpretErrorCode(code);
            result.Text = ErrorCodeToText(code);
            return result;
        }
    }
}
