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
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Serialization
{
    internal sealed partial class WorldSerializer
    {
        private readonly ActorMapper actorMapper;
        private readonly ContentMapper contentMapper;
        private readonly EnvironmentMapper environmentMapper;
        private readonly ManifestMapper manifestMapper;
        private readonly PropertyMapper propertyMapper;
        private readonly SlotLayerMapper slotLayerMapper;
        private readonly SlotMapper slotMapper;
        private readonly Texture2DMapper texture2DMapper;

        private readonly ActorManager actorManager;
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly DataSerializer dataSerializer;
        private readonly World world;

        private readonly Dictionary<Type, string> typeToEntryName;
        private readonly Dictionary<Type, IMapper> typeToMapper;
        private readonly Dictionary<Type, MigrationRegistry> typeToMigrationRegistry;

        internal WorldSerializer(ActorManager actorManager, GraphicsDeviceManager graphicsDeviceManager, DataSerializer dataSerializer, World world)
        {
            this.actorManager = actorManager;
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.dataSerializer = dataSerializer;
            this.world = world;

            this.actorMapper = new();
            this.slotLayerMapper = new();
            this.slotMapper = new(this.slotLayerMapper);
            this.contentMapper = new(this.actorMapper, this.slotMapper);
            this.environmentMapper = new();
            this.manifestMapper = new();
            this.propertyMapper = new();
            this.texture2DMapper = new();

            this.typeToEntryName = new()
            {
                [typeof(ContentStorageModel)] = IOConstants.SAVE_ENTRY_CONTENT_FILE,
                [typeof(EnvironmentStorageModel)] = IOConstants.SAVE_ENTRY_ENVIRONMENT_FILE,
                [typeof(ManifestStorageModel)] = IOConstants.SAVE_ENTRY_MANIFEST_FILE,
                [typeof(PropertyStorageModel)] = IOConstants.SAVE_ENTRY_PROPERTIES_FILE,
                [typeof(Texture2DStorageModel)] = IOConstants.SAVE_ENTRY_THUMBNAIL_FILE
            };

            this.typeToMapper = new()
            {
                [typeof(ContentStorageModel)] = this.contentMapper,
                [typeof(EnvironmentStorageModel)] = this.environmentMapper,
                [typeof(ManifestStorageModel)] = this.manifestMapper,
                [typeof(PropertyStorageModel)] = this.propertyMapper,
                [typeof(Texture2DStorageModel)] = this.texture2DMapper
            };

            this.typeToMigrationRegistry = new()
            {
                [typeof(ContentStorageModel)] = new(),
                [typeof(EnvironmentStorageModel)] = new(),
                [typeof(ManifestStorageModel)] = new(),
                [typeof(PropertyStorageModel)] = new(),
                [typeof(Texture2DStorageModel)] = new()
            };
        }

        private void Serialize<TMapper, TStorageModel>(ZipArchive zip, string entryName, TMapper mapper, TStorageModel storageModel)
            where TMapper : IMapper
            where TStorageModel : IStorageModel
        {
            ZipArchiveEntry entry = zip.CreateEntry(entryName, CompressionLevel.SmallestSize);
            using Stream stream = entry.Open();

            this.dataSerializer.Serialize(stream, mapper, storageModel);
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

            Serialize(zip, IOConstants.SAVE_ENTRY_CONTENT_FILE, this.contentMapper, SerializeContent());
            Serialize(zip, IOConstants.SAVE_ENTRY_ENVIRONMENT_FILE, this.environmentMapper, SerializeEnvironment());
            Serialize(zip, IOConstants.SAVE_ENTRY_MANIFEST_FILE, this.manifestMapper, SerializeManifest());
            Serialize(zip, IOConstants.SAVE_ENTRY_PROPERTIES_FILE, this.propertyMapper, SerializeProperties());
            Serialize(zip, IOConstants.SAVE_ENTRY_THUMBNAIL_FILE, this.texture2DMapper, SerializeThumbnail());
        }

        private TStorageModel Deserialize<TStorageModel>(ZipArchive zip, string entryName, IMapper mapper, MigrationRegistry migrationRegistry, int sourceVersion, int targetVersion)
            where TStorageModel : IStorageModel
        {
            ZipArchiveEntry entry = zip.GetEntry(entryName);

            if (entry == null)
            {
                return default;
            }

            using Stream stream = entry.Open();
            return this.dataSerializer.Deserialize<TStorageModel>(stream, mapper, migrationRegistry, sourceVersion, targetVersion);
        }

        internal TStorageModel Load<TStorageModel>(string name)
            where TStorageModel : IStorageModel
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(name, IOConstants.SAVE_FILE_EXTENSION));
            
            using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
            using ZipArchive zip = new(fs, ZipArchiveMode.Read);

            Type storageModelType = typeof(TStorageModel);

            string entryName = this.typeToEntryName[storageModelType];
            IMapper mapper = this.typeToMapper[storageModelType];
            MigrationRegistry migrationRegistry = this.typeToMigrationRegistry[storageModelType];

            int sourceVersion = 1;
            int targetVersion = 1;

            return Deserialize<TStorageModel>(zip, entryName, mapper, migrationRegistry, sourceVersion, targetVersion);
        }
    }
}

