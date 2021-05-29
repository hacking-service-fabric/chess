using Chess.Data.Common.Models.V1;

namespace Chess.Queue.SMS.Models
{
    public class SmsModel
    {
        public ConversationDto Conversation { get; set; }
        public MessageDto Message { get; set; }
    }
}
