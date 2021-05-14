using System.Collections.Generic;
using PhoneNumbers;

namespace Chess.Data.Common.Models.V1
{
    public class ConversationDto
    {
        public PhoneNumber HostPhoneNumber { get; set; }
        public IEnumerable<PhoneNumber> PhoneNumbers { get; set; }
    }
}
