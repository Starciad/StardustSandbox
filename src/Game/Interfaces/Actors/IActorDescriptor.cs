using StardustSandbox.Actors;
using StardustSandbox.Enums.Actors;

namespace StardustSandbox.Interfaces.Actors
{
    internal interface IActorDescriptor
    {
        ActorIndex Index { get; }

        Actor Create();
        void Recycle(Actor actor);

        byte[] Serialize(Actor actor);
        Actor Deserialize(byte[] data);
    }
}
