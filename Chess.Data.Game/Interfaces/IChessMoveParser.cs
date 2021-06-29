using Chess.Data.Common.Models.V1;

namespace Chess.Data.Game.Interfaces
{
    public interface IChessMoveParser<T>
        where T: MoveDtoBase
    {
        bool TryParse(string message, out T result);
    }

    public interface IChessMoveParser: IChessMoveParser<MoveDtoBase>
    { }
}
