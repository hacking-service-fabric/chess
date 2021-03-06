using Chess.Data.Common;
using Chess.Data.Common.Models.V1;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using PhoneNumbers;
using System.Threading.Tasks;
using Chess.Data.Game.Interfaces;

namespace Chess.Data.Game
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class Game : Actor, IGame
    {
        private readonly IChessMoveParser _moveParser;

        /// <summary>
        /// Initializes a new instance of Game
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Game(ActorService actorService, ActorId actorId,
            IChessMoveParser moveParser
        ) : base(actorService, actorId)
        {
            _moveParser = moveParser;
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            return this.StateManager.TryAddStateAsync("count", 0);
        }

        public async Task<MoveResultDtoBase> TryMove(MessageDto message)
        {
            PhoneNumber some = null;
            if (message.FromPhoneNumber.Equals(some) &&_moveParser.TryParse(message.Text, out var move))
            {
            }

            return NoReplyDto.Instance;
        }
    }
}
