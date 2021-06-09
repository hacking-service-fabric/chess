using Chess.Data.Common.Models.V1;

namespace Chess.Queue.Move.Model
{
    class MessageModel
    {
        public ConversationDto Conversation { get; set; }
        public ushort ConversationMessageId { get; set; }
    }
}
