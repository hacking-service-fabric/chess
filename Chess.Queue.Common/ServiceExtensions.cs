using Chess.Queue.Common.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Chess.Queue.Common
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSmsQueueService(this IServiceCollection services)
            => services.AddSingleton<ISmsQueueServiceAccessor, SmsQueueServiceAccessor>();

        public static IServiceCollection AddMoveQueueService(this IServiceCollection services)
            => services.AddSingleton<IMoveQueueServiceAccessor, MoveQueueServiceAccessor>();
    }
}
