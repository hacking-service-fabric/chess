using Chess.Data.Common.Models.V1;
using PhoneNumbers;

namespace Chess.Queue.Common.Models
{
    public class MoveModel
    {
        public ConversationDto Conversation { get; set; }
        public PhoneNumber FromPhoneNumber { get; set; }
        public MoveDescription Description { get; set; }
        public MovePosition FromPosition { get; set; }
        public MovePosition ToPosition { get; set; }
    }
}
