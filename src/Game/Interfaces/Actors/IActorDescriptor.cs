using StardustSandbox.Actors;
using StardustSandbox.Enums.Actors;

namespace StardustSandbox.Interfaces.Actors
{
    internal interface IActorDescriptor
    {
        ActorIndex Index { get; }

        Actor Create();
        void Destroy(Actor actor);
    }
}
