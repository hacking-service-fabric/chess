using System.Runtime.Serialization;

namespace Chess.Data.Common.Models.V1
{
    [DataContract]
    [KnownType(typeof(NoReplyDto))]
    public abstract class MoveResultDtoBase
    {
    }
}
