using Chess.Queue.Common.Interfaces;
using System.Collections.Generic;
using PhoneNumbers;

namespace Chess.Queue.Common
{
    public interface IMoveQueueServiceAccessor
    {
        IMoveQueueService GetInstance(IEnumerable<PhoneNumber> recipientPhoneNumbers);
    }
}
