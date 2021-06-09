namespace Chess.Data.Common.Models.V1
{
    public class NoReplyDto: MoveResultDtoBase
    {
        private static NoReplyDto _instance;
        public static NoReplyDto Instance => _instance ??= new NoReplyDto();

        private NoReplyDto()
        { }
    }
}
