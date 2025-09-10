using MediatR;

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Log
{
    public class UILogNotificatinon : INotification
    {
        public LogMessage LogMessage { get; set; }

        public UILogNotificatinon()
        {
        }

        public UILogNotificatinon(LogMessage msg)
        {
            LogMessage = msg;
        }
    }
}
