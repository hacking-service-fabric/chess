using System;
using PhoneNumbers;

namespace Chess.Data.Common.Models.V1
{
    public class MessageDto
    {
        public DateTime MessageTime { get; set; }
        public PhoneNumber FromPhoneNumber { get; set; }
        public string Text { get; set; }
    }
}
