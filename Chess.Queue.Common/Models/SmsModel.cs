using Chess.Data.Common.Models;

namespace Chess.Queue.Common.Models
{
    public class SmsModel
    {
        public Conversation Conversation { get; set; }
        public string TextContent { get; set; }
    }
}
