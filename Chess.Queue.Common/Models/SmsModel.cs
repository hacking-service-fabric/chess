using System.Collections.Generic;

namespace Chess.Queue.Common.Models
{
    public class SmsModel
    {
        public IEnumerable<int> Recipients { get; set; }
        public string TextContent { get; set; }
    }
}
