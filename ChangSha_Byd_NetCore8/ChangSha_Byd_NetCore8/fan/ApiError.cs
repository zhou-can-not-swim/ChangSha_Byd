namespace ChangSha_Byd_NetCore8.fan
{
    public struct ApiError
    {
        public string DevName { get; set; }

        public ApiErrorCode Error { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return $"{DevName}: {Text}：{Error}";
        }
    }

    public struct ApiErrorCode
    {
        public short OsSocketError;

        public IsoTcpErrors IsoTcpError;

        public short S7Error;

        public override string ToString()
        {
            return $"OsSockerError={OsSocketError};IsoTcpError={IsoTcpError};S7Error={S7Error}";
        }
    }

    public enum IsoTcpErrors : byte
    {
        None,
        ConnectionError,
        DisconnectionError,
        MalformattedPDUSuppled,
        BadDatasizePassedToSendRecv,
        NullPointerSupplied,
        ShortPacketReceived,
        TooManyFragments,
        PduOverflow,
        SendPacketError,
        RecvPacketError,
        InvalidParams,
        Resvd1,
        Resvd2,
        Resvd3,
        Resvd4
    }
}
