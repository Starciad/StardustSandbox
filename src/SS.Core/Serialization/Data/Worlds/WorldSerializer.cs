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

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.Serialization.Migrations;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Data;
using StardustSandbox.Core.Serialization.Data.Worlds.Mappers;
using StardustSandbox.Core.Serialization.Data.Worlds.StorageModels;
using StardustSandbox.Core.Serialization.Migrations;
using StardustSandbox.Core.WorldSystem;

using System;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Serialization
{
    internal sealed partial class WorldSerializer
    {
        private readonly MigrationRegistry migrationRegistry;

        private readonly ActorMapper actorMapper;
        private readonly ContentMapper contentMapper;
        private readonly EnvironmentMapper environmentMapper;
        private readonly ManifestMapper manifestMapper;
        private readonly PropertyMapper propertyMapper;
        private readonly SlotLayerMapper slotLayerMapper;
        private readonly SlotMapper slotMapper;
        private readonly Texture2DMapper texture2DMapper;
        private readonly VersionMapper versionMapper;

        private readonly ActorManager actorManager;
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly DataSerializer dataSerializer;
        private readonly World world;

        internal WorldSerializer(ActorManager actorManager, GraphicsDeviceManager graphicsDeviceManager, DataSerializer dataSerializer, World world)
        {
            this.actorManager = actorManager;
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.dataSerializer = dataSerializer;
            this.world = world;

            this.migrationRegistry = new();

            this.actorMapper = new();
            this.slotLayerMapper = new();
            this.slotMapper = new(this.slotLayerMapper);
            this.contentMapper = new(this.actorMapper, this.slotMapper);
            this.environmentMapper = new();
            this.manifestMapper = new();
            this.propertyMapper = new();
            this.texture2DMapper = new();
            this.versionMapper = new();
        }

        private void Write<T>(ZipArchive zip, string entryName, T data)
        {
            ZipArchiveEntry entry = zip.CreateEntry(entryName, CompressionLevel.SmallestSize);
            using Stream stream = entry.Open();
            MessagePackSerializer.Serialize(stream, data, this.options);
        }

        internal void Save()
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(this.world.Name, IOConstants.SAVE_FILE_EXTENSION));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using FileStream fs = new(filename, FileMode.Create, FileAccess.Write);
            using ZipArchive zip = new(fs, ZipArchiveMode.Create);

            Write(zip, IOConstants.SAVE_ENTRY_CONTENT_FILE, SerializeContent());
            Write(zip, IOConstants.SAVE_ENTRY_ENVIRONMENT_FILE, SerializeEnvironment());
            Write(zip, IOConstants.SAVE_ENTRY_MANIFEST_FILE, SerializeManifest());
            Write(zip, IOConstants.SAVE_ENTRY_PROPERTIES_FILE, SerializeProperties());
            Write(zip, IOConstants.SAVE_ENTRY_THUMBNAIL_FILE, SerializeThumbnail());
            Write(zip, IOConstants.SAVE_ENTRY_VERSION_FILE, SerializeVersion());
        }

        private T Read<T>(ZipArchive zip, string entryName) where T : IStorageModel
        {
            ZipArchiveEntry entry = zip.GetEntry(entryName);

            if (entry == null)
            {
                return default;
            }

            using Stream stream = entry.Open();
            return MessagePackSerializer.Deserialize<T>(stream, this.options);
        }

        internal T Load<T>(string name) where T : IStorageModel
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(name, IOConstants.SAVE_FILE_EXTENSION));
            
            using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
            using ZipArchive zip = new(fs, ZipArchiveMode.Read);

            Type storageModelType = typeof(T);

            VersionStorageModel versionStorageModel = Read<VersionStorageModel>(zip, IOConstants.SAVE_ENTRY_VERSION_FILE);

            // 1 - Convert storage model to the data type
            // 2 - Throw exception if the version file is missing
            // 3 - Deserialize the version of component
            // 4 - Migrate the data to the latest version if necessary
            // 5 - Deserialize the data
            // 6 - Return the deserialized data

            return default;
        }
    }
}

