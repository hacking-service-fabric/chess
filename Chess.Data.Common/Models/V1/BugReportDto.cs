using System;

namespace Chess.Data.Common.Models.V1
{
    public class BugReportDto
    {
        public ConversationDto Conversation { get; set; }
        public DateTime ReportTime { get; set; }
    }
}
