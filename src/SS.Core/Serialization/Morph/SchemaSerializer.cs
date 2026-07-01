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

using StardustSandbox.Core.Interfaces.Serialization.Morph;

using System;
using System.IO;

namespace StardustSandbox.Core.Serialization.Morph
{
    internal sealed class SchemaSerializer
    {
        private readonly Func<Stream, Type, IData> deserializer;
        private readonly Action<Stream, IData> serializer;

        internal SchemaSerializer(Func<Stream, Type, IData> deserializer, Action<Stream, IData> serializer)
        {
            this.deserializer = deserializer;
            this.serializer = serializer;
        }

        internal void Serialize(Stream stream, IMapper mapper, IStorageModel storageModel)
        {
            this.serializer(stream, mapper.ToData(storageModel));
        }

        private static void Migrate(ref IData data, ComponentSchema componentSchema, int sourceVersion, int targetVersion)
        {
            if (sourceVersion >= targetVersion)
            {
                return;
            }

            foreach (IMigration migration in componentSchema.GetMigrations(sourceVersion, targetVersion))
            {
                data = migration.Migrate(data);
            }
        }

        internal TStorageModel Deserialize<TStorageModel>(Stream stream, ComponentSchema componentSchema, int sourceVersion, int targetVersion) where TStorageModel : IStorageModel
        {
            IData data = this.deserializer(stream, componentSchema.GetVersionType(sourceVersion));
            Migrate(ref data, componentSchema, sourceVersion, targetVersion);
            return (TStorageModel)componentSchema.Mapper.ToStorageModel(data);
        }
    }
}
