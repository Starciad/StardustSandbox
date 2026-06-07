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

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Serialization.Migrations;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Data.Versions;
using StardustSandbox.Core.Serialization.Data.Worlds.Mappers;
using StardustSandbox.Core.Serialization.Data.Worlds.StorageModels;
using StardustSandbox.Core.Serialization.Migrations;
using StardustSandbox.Core.WorldSystem;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Serialization.Data.Worlds
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
        private readonly VersionMapper versionMapper;

        private readonly ActorManager actorManager;
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly DataSerializer dataSerializer;
        private readonly World world;

        private readonly Dictionary<Type, string> entryNames;
        private readonly Dictionary<Type, IMapper> mappers;
        private readonly Dictionary<Type, MigrationRegistry> migrationRegistry;
        private readonly Dictionary<Type, int> targetVersions;

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
            this.versionMapper = new();

            this.entryNames = new()
            {
                [typeof(ContentStorageModel)] = IOConstants.SAVE_ENTRY_CONTENT_FILE,
                [typeof(EnvironmentStorageModel)] = IOConstants.SAVE_ENTRY_ENVIRONMENT_FILE,
                [typeof(ManifestStorageModel)] = IOConstants.SAVE_ENTRY_MANIFEST_FILE,
                [typeof(PropertyStorageModel)] = IOConstants.SAVE_ENTRY_PROPERTIES_FILE,
                [typeof(Texture2DStorageModel)] = IOConstants.SAVE_ENTRY_THUMBNAIL_FILE
            };

            this.mappers = new()
            {
                [typeof(ContentStorageModel)] = this.contentMapper,
                [typeof(EnvironmentStorageModel)] = this.environmentMapper,
                [typeof(ManifestStorageModel)] = this.manifestMapper,
                [typeof(PropertyStorageModel)] = this.propertyMapper,
                [typeof(Texture2DStorageModel)] = this.texture2DMapper,
                [typeof(VersionStorageModel)] = this.versionMapper
            };

            this.migrationRegistry = new()
            {
                [typeof(ContentStorageModel)] = new(),
                [typeof(EnvironmentStorageModel)] = new(),
                [typeof(ManifestStorageModel)] = new(),
                [typeof(PropertyStorageModel)] = new(),
                [typeof(Texture2DStorageModel)] = new()
            };

            this.targetVersions = new()
            {
                [typeof(ContentStorageModel)] = IOConstants.SAVE_CONTENT_COMPONENT_VERSION,
                [typeof(EnvironmentStorageModel)] = IOConstants.SAVE_ENVIRONMENT_COMPONENT_VERSION,
                [typeof(ManifestStorageModel)] = IOConstants.SAVE_MANIFEST_COMPONENT_VERSION,
                [typeof(PropertyStorageModel)] = IOConstants.SAVE_PROPERTIES_COMPONENT_VERSION,
                [typeof(Texture2DStorageModel)] = IOConstants.SAVE_THUMBNAIL_COMPONENT_VERSION
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

        #region

        private ContentStorageModel CreateContent()
        {
            return new()
            {
                Actors = this.actorManager.Serialize(),
                Slots = this.world.SerializationHelper.Serialize(),
            };
        }

        private EnvironmentStorageModel CreateEnvironment()
        {
            return new()
            {
                CurrentTime = this.world.Time.CurrentTime,
                IsFrozen = this.world.Time.IsFrozen,
            };
        }

        private ManifestStorageModel CreateManifest()
        {
            return new()
            {
                CreationTimestamp = DateTime.Now,
                Description = this.world.Description,
                LastModifiedTimestamp = DateTime.Now,
                Name = this.world.Name,
            };
        }

        private PropertyStorageModel CreateProperties()
        {
            return new()
            {
                Width = this.world.TileMap.Width,
                Height = this.world.TileMap.Height,
            };
        }

        private Texture2DStorageModel CreateThumbnail()
        {
            return new(this.world.TileMap.CreateThumbnail(this.graphicsDeviceManager.GraphicsDevice));
        }

        private static VersionStorageModel CreateVersion()
        {
            return new()
            {
                Components = new Dictionary<string, int>()
                {
                    [IOConstants.SAVE_ENTRY_CONTENT_ID] = IOConstants.SAVE_CONTENT_COMPONENT_VERSION,
                    [IOConstants.SAVE_ENTRY_ENVIRONMENT_ID] = IOConstants.SAVE_ENVIRONMENT_COMPONENT_VERSION,
                    [IOConstants.SAVE_ENTRY_MANIFEST_ID] = IOConstants.SAVE_MANIFEST_COMPONENT_VERSION,
                    [IOConstants.SAVE_ENTRY_PROPERTIES_ID] = IOConstants.SAVE_PROPERTIES_COMPONENT_VERSION,
                    [IOConstants.SAVE_ENTRY_THUMBNAIL_ID] = IOConstants.SAVE_THUMBNAIL_COMPONENT_VERSION,
                }
            };
        }

        #endregion

        internal void Save()
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(this.world.Name, IOConstants.SAVE_FILE_EXTENSION));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using FileStream fs = new(filename, FileMode.Create, FileAccess.Write);
            using ZipArchive zip = new(fs, ZipArchiveMode.Create);

            Serialize(zip, IOConstants.SAVE_ENTRY_CONTENT_FILE, this.contentMapper, CreateContent());
            Serialize(zip, IOConstants.SAVE_ENTRY_ENVIRONMENT_FILE, this.environmentMapper, CreateEnvironment());
            Serialize(zip, IOConstants.SAVE_ENTRY_MANIFEST_FILE, this.manifestMapper, CreateManifest());
            Serialize(zip, IOConstants.SAVE_ENTRY_PROPERTIES_FILE, this.propertyMapper, CreateProperties());
            Serialize(zip, IOConstants.SAVE_ENTRY_THUMBNAIL_FILE, this.texture2DMapper, CreateThumbnail());
            Serialize(zip, IOConstants.SAVE_ENTRY_VERSION_FILE, this.versionMapper, CreateVersion());
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

            VersionStorageModel sourceVersions = Deserialize<VersionStorageModel>(zip, IOConstants.SAVE_ENTRY_VERSION_FILE, this.versionMapper, null, 0, 0);
          
            Type storageModelType = typeof(TStorageModel);

            string entryName = this.entryNames[storageModelType];
            IMapper mapper = this.mappers[storageModelType];
            MigrationRegistry migrationRegistry = this.migrationRegistry[storageModelType];

            int sourceVersion = sourceVersions.Components[entryName];
            int targetVersion = this.targetVersions[storageModelType];

            return Deserialize<TStorageModel>(zip, entryName, mapper, migrationRegistry, sourceVersion, targetVersion);
        }
    }
}

