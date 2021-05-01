using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Data.Common.Models
{
    public class BugReport
    {
        public IConversation Conversation { get; set; }
        public DateTime ReportTime { get; set; }
    }
}
