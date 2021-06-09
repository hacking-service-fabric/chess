using Chess.Data.Common.Models.V1;

namespace Chess.Data.Game.Implementations
{
    public class ChessMoveParser: IChessMoveParser
    {
        public bool TryParse(string message, out MoveDtoBase result)
        {
            if (MoveResignDto.TryParse(message, out var moveResign))
            {
                result = moveResign;
                return true;
            }

            if (MoveCastleDto.TryParse(message, out var moveCastle))
            {
                result = moveCastle;
                return true;
            }

            if (MovePromotionDto.TryParse(message, out var movePromotion))
            {
                result = movePromotion;
                return true;
            }

            if (MovePieceDto.TryParse(message, out var movePiece))
            {
                result = movePiece;
                return true;
            }

            result = null;
            return false;
        }
    }
}
