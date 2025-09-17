namespace ChangSha_Byd_NetCore8.Controllers.Dto
{
    // 如果字段很多，可以用 record，省写构造函数
    public class PlcDto
    {
        public int CurrentColumn { get; set; }
        public int CurrentFloor { get; set; }
        public int CurrentLine { get; set; }
        public int DoTaskNo { get; set; }

        public Dictionary<string, PortDto> EC010_A库口 { get; set; } = new();
        public Dictionary<string, PortDto> EC010_B库口 { get; set; } = new();

        public string ErrorCode1 { get; set; } = string.Empty;
        public string ErrorCode2 { get; set; } = string.Empty;
        public string ErrorCode3 { get; set; } = string.Empty;
        public string ErrorCode4 { get; set; } = string.Empty;
        public string ErrorCode5 { get; set; } = string.Empty;

        public bool FinishTaskReq { get; set; }
        public bool RequestOutTaskAck { get; set; }
        public bool RequestTaskAck { get; set; }
        public bool RequestTaskResultReq { get; set; }
        public bool SendTaskAck { get; set; }

        public int StockerAction { get; set; }
        public int StockerCargo { get; set; }
        public int StockerTrip { get; set; }
        public int StorkerStatus { get; set; }
        public int TaskNo { get; set; }
        public int VerificationCode { get; set; }

        public bool HeartBeatAck { get; set; }
        public bool HeartBeatReq { get; set; }
    }

    public class PortDto
    {
        public string EntryRFID { get; set; } = "0";
        public bool OutStatus { get; set; }
        public int RequestTaskResult { get; set; }
        public bool StandByReq { get; set; }
    }

    public class MstDto
    {
        public Dictionary<string, MstPortDto> EC010_A库口 { get; set; } = new();
        public Dictionary<string, MstPortDto> EC010_B库口 { get; set; } = new();

        public int EndColumn { get; set; }
        public int EndFloor { get; set; }
        public int EndLine { get; set; }
        public bool FinishTaskAck { get; set; }
        public bool RequsetOutTaskReq { get; set; }
        public bool RequsetTaskReq { get; set; }
        public bool RequsetTaskResultAck { get; set; }
        public bool SendTaskReq { get; set; }
        public int StartColumn { get; set; }
        public int StartFloor { get; set; }
        public int StartLine { get; set; }
        public int TaskNo { get; set; }
        public int TaskRFID { get; set; }
        public int TaskType { get; set; }
        public int VerificationCode { get; set; }
        public bool HeartBeatAck { get; set; }
        public bool HeartBeatReq { get; set; }
    }

    public record MstPortDto
    {
        public string RequestTaskRFID { get; set; } = "0";
        public bool StandByAck { get; set; }
    }

    // 统一入口
    public class AllContextDto(PlcDto Plc, MstDto Mst);
}
