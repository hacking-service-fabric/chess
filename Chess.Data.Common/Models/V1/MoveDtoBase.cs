using System.Runtime.Serialization;

namespace Chess.Data.Common.Models.V1
{
    [DataContract]
    [KnownType(typeof(MoveResignDto))]
    [KnownType(typeof(MoveCastleDto))]
    [KnownType(typeof(MovePromotionDto))]
    [KnownType(typeof(MovePieceDto))]
    public abstract class MoveDtoBase
    { }
}
