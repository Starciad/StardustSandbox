/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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

