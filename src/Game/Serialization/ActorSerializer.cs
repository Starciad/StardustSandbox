using MessagePack;
using MessagePack.Resolvers;

using StardustSandbox.Actors;
using StardustSandbox.Databases;
using StardustSandbox.Serialization.Saving.Data;

namespace StardustSandbox.Serialization
{
    internal static class ActorSerializer
    {
        private static readonly MessagePackSerializerOptions options =
            MessagePackSerializerOptions.Standard
                .WithResolver(StandardResolver.Instance)
                .WithSecurity(MessagePackSecurity.UntrustedData)
                .WithCompression(MessagePackCompression.Lz4BlockArray);

        internal static byte[] Serialize(Actor actor)
        {
            return MessagePackSerializer.Serialize(actor.Serialize(), options);
        }

        internal static Actor Deserialize(byte[] bytes)
        {
            ActorData data = MessagePackSerializer.Deserialize<ActorData>(bytes, options);

            Actor actor = ActorDatabase.GetDescriptor(data.Index).Dequeue();
            actor.Deserialize(data);

            return actor;
        }
    }
}
