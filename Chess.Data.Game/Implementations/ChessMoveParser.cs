using Chess.Data.Common.Models.V1;
using Chess.Data.Game.Interfaces;

namespace Chess.Data.Game.Implementations
{
    public class ChessMoveParser: IChessMoveParser
    {
        private readonly IChessMoveParser<MoveResignDto> _resignParser;
        private readonly IChessMoveParser<MoveCastleDto> _castleParser;
        private readonly IChessMoveParser<MovePromotionDto> _promotionParser;
        private readonly IChessMoveParser<MovePieceDto> _pieceParser;

        public ChessMoveParser(
            IChessMoveParser<MoveResignDto> resignParser,
            IChessMoveParser<MoveCastleDto> castleParser,
            IChessMoveParser<MovePromotionDto> promotionParser,
            IChessMoveParser<MovePieceDto> pieceParser)
        {
            _resignParser = resignParser;
            _castleParser = castleParser;
            _promotionParser = promotionParser;
            _pieceParser = pieceParser;
        }

        public bool TryParse(string message, out MoveDtoBase result)
        {
            if (_resignParser.TryParse(message, out var moveResign))
            {
                result = moveResign;
            }
            else if (_castleParser.TryParse(message, out var moveCastle))
            {
                result = moveCastle;
            }
            else if (_promotionParser.TryParse(message, out var movePromotion))
            {
                result = movePromotion;
            }
            else if (_pieceParser.TryParse(message, out var movePiece))
            {
                result = movePiece;
            }
            else
            {
                result = null;
                return false;
            }

            return true;
        }
    }
}
