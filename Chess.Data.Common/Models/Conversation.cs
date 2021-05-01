using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneNumbers;

namespace Chess.Data.Common.Models
{
    public class Conversation
    {
        public PhoneNumber HostPhoneNumber { get; set; }
        public IEnumerable<PhoneNumber> RecipientPhoneNumbers { get; set; }
    }
}
