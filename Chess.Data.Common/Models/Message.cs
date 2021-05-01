using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneNumbers;

namespace Chess.Data.Common.Models
{
    public class Message
    {
        public DateTime MessageTime { get; set; }
        public PhoneNumber SenderPhoneNumber { get; set; }
        public string Text { get; set; }
    }
}
