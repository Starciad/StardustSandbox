using MessagePack;

using StardustSandbox.Actors;
using StardustSandbox.Databases;
using StardustSandbox.Interfaces.Actors;
using StardustSandbox.Serialization.Saving.Data;

namespace StardustSandbox.Serialization
{
    internal static class ActorSerializer
    {
        internal static byte[] Serialize(Actor actor)
        {
            return MessagePackSerializer.Serialize(actor.Serialize(), SavingSerializer.Options);
        }

        internal static Actor Deserialize(byte[] bytes)
        {
            ActorData data = MessagePackSerializer.Deserialize<ActorData>(bytes, SavingSerializer.Options);
            IActorDescriptor descriptor = ActorDatabase.GetDescriptor(data.Index);

            Actor actor = descriptor.Dequeue();
            actor.Deserialize(data);

            return actor;
        }
    }
}
