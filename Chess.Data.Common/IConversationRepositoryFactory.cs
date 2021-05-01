using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IConversationRepositoryFactory
    {
        Task<IConversationRepository> GetRepository();
    }
}
