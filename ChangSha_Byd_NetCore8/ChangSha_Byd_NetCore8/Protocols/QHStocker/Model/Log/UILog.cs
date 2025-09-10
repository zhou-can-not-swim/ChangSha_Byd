namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Log
{
    public class LogMessage
    {
        public DateTime Timestamp { get; set; }

        public LogLevel Level { get; set; }

        public string Content { get; set; }
    }
}
