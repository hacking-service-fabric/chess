namespace Chess.Data.Common.Models.V1
{
    public class MoveResignDto: MoveDtoBase
    {
        public static bool TryParse(string message, out MoveResignDto result)
        {
            if (message.Trim().ToLower() == "gg")
            {
                result = new MoveResignDto();
                return true;
            }

            result = null;
            return false;
        }
    }
}
