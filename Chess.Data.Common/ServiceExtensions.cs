using Chess.Data.Common.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace Chess.Data.Common
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConversationRepository(this IServiceCollection services)
            => services.AddSingleton<IConversationRepository, ConversationRepositoryAccessor>();
    }
}
