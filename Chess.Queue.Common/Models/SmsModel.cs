using Chess.Data.Common.Models.V1;

namespace Chess.Queue.Common.Models
{
    public class SmsModel
    {
        public ConversationDto Conversation { get; set; }
        public string TextContent { get; set; }
    }
}
