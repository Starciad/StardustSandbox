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

using StardustSandbox.Core.Interfaces.Serialization.Migrations;
using StardustSandbox.Core.Serialization.Migrations;

using System.IO;

namespace StardustSandbox.Core.Serialization.Data
{
    internal sealed class DataSerializer
    {
        private readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard
            .WithResolver(CompositeResolver.Create(StandardResolver.Instance))
            .WithSecurity(MessagePackSecurity.UntrustedData)
            .WithCompression(MessagePackCompression.Lz4BlockArray);

        internal void Serialize<TMapper, TStorageModel>(Stream stream, TMapper mapper, TStorageModel storageModel)
            where TMapper : IMapper
            where TStorageModel : IStorageModel
        {
            IData data = mapper.ToData(storageModel);
            MessagePackSerializer.Serialize(stream, data, this.options);
        }

        internal TStorageModel Deserialize<TData, TMapper, TStorageModel>(Stream stream, TMapper mapper, MigrationRegistry migrationRegistry, int sourceVersion, int targetVersion)
            where TData : IData
            where TMapper : IMapper
            where TStorageModel : IStorageModel
        {
            // Deserialize Version (DATA V1, V2, [...])
            TData data = MessagePackSerializer.Deserialize<TData>(stream, this.options);

            // Migrate
            int currentVersion = sourceVersion;

            while (currentVersion < targetVersion)
            {
                migrationRegistry.GetMigration(currentVersion).Migrate(data);
                currentVersion++;
            }

            // Data to StorageModel
            return (TStorageModel)mapper.ToStorageModel(data);
        }
    }
}
