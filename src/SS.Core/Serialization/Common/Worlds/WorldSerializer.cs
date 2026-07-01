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
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Common.Worlds.Mappers;
using StardustSandbox.Core.Serialization.Common.Worlds.StorageModels;
using StardustSandbox.Core.Serialization.Morph;
using StardustSandbox.Core.WorldSystem;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace StardustSandbox.Core.Serialization.Common.Worlds
{
    internal sealed partial class WorldSerializer
    {
        private readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard
            .WithResolver(CompositeResolver.Create(StandardResolver.Instance))
            .WithSecurity(MessagePackSecurity.UntrustedData)
            .WithCompression(MessagePackCompression.Lz4BlockArray);

        private readonly ActorMapper actorMapper;
        private readonly ContentMapper contentMapper;
        private readonly EnvironmentMapper environmentMapper;
        private readonly ManifestMapper manifestMapper;
        private readonly PropertyMapper propertyMapper;
        private readonly SlotLayerMapper slotLayerMapper;
        private readonly SlotMapper slotMapper;
        private readonly Texture2DMapper texture2DMapper;

        private readonly ComponentSchema actorComponentSchema;
        private readonly ComponentSchema contentComponentSchema;
        private readonly ComponentSchema environmentComponentSchema;
        private readonly ComponentSchema manifestComponentSchema;
        private readonly ComponentSchema propertyComponentSchema;
        private readonly ComponentSchema slotLayerComponentSchema;
        private readonly ComponentSchema slotComponentSchema;
        private readonly ComponentSchema texture2DComponentSchema;

        private readonly Dictionary<Type, ComponentSchema> componentSchemasByType;
        private readonly Dictionary<Type, int> componentVersionsByType = new()
        {
            [typeof(ContentStorageModel)] = IOConstants.SAVE_CONTENT_COMPONENT_VERSION,
            [typeof(EnvironmentStorageModel)] = IOConstants.SAVE_ENVIRONMENT_COMPONENT_VERSION,
            [typeof(ManifestStorageModel)] = IOConstants.SAVE_MANIFEST_COMPONENT_VERSION,
            [typeof(PropertyStorageModel)] = IOConstants.SAVE_PROPERTIES_COMPONENT_VERSION,
            [typeof(Texture2DStorageModel)] = IOConstants.SAVE_THUMBNAIL_COMPONENT_VERSION,
        };
        private readonly Dictionary<Type, string> componentEntryNamesByType = new()
        {
            [typeof(ContentStorageModel)] = IOConstants.SAVE_ENTRY_CONTENT_FILE,
            [typeof(EnvironmentStorageModel)] = IOConstants.SAVE_ENTRY_ENVIRONMENT_FILE,
            [typeof(ManifestStorageModel)] = IOConstants.SAVE_ENTRY_MANIFEST_FILE,
            [typeof(PropertyStorageModel)] = IOConstants.SAVE_ENTRY_PROPERTIES_FILE,
            [typeof(Texture2DStorageModel)] = IOConstants.SAVE_ENTRY_THUMBNAIL_FILE,
        };

        private readonly ActorManager actorManager;
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly World world;

        private readonly SchemaSerializer schemaSerializer;

        private IData Deserializer(Stream stream, Type versionType)
        {
            return (IData)MessagePackSerializer.Deserialize(versionType, stream, this.options);
        }

        private void Serializer(Stream stream, IData data)
        {
            MessagePackSerializer.Serialize(stream, data, this.options);
        }

        internal WorldSerializer(ActorManager actorManager, GraphicsDeviceManager graphicsDeviceManager, World world) : base()
        {
            this.actorManager = actorManager;
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.world = world;

            this.actorMapper = new();
            this.slotLayerMapper = new();
            this.slotMapper = new(this.slotLayerMapper);
            this.contentMapper = new(this.actorMapper, this.slotMapper);
            this.environmentMapper = new();
            this.manifestMapper = new();
            this.propertyMapper = new();
            this.texture2DMapper = new();

            this.actorComponentSchema = new(
                mapper: this.actorMapper,
                migrations: [],
                versionTypes: [
                    typeof(Data.V1.ActorData)
                ]
            );

            this.contentComponentSchema = new(
                mapper: this.contentMapper,
                migrations: [],
                versionTypes: [
                    typeof(Data.V1.ContentData)
                ]
            );

            this.environmentComponentSchema = new(
                mapper: this.environmentMapper,
                migrations: [],
                versionTypes: [
                    typeof(Data.V1.EnvironmentData)
                ]
            );

             this.manifestComponentSchema = new(
                 mapper: this.manifestMapper,
                 migrations: [],
                 versionTypes: [
                     typeof(Data.V1.ManifestData)
                 ]
             );

            this.propertyComponentSchema = new(
                mapper: this.propertyMapper,
                migrations: [],
                versionTypes: [
                    typeof(Data.V1.PropertyData)
                ]
            );

            this.slotLayerComponentSchema = new(
                mapper: this.slotLayerMapper,
                migrations: [],
                versionTypes: [
                    typeof(Data.V1.SlotLayerData)
                ]
            );

            this.slotComponentSchema = new(
                mapper: this.slotMapper,
                migrations: [],
                versionTypes: [
                    typeof(Data.V1.SlotData)
                ]
            );

            this.texture2DComponentSchema = new(
                mapper: this.texture2DMapper,
                migrations: [],
                versionTypes: [
                    typeof(Data.V1.Texture2DData)
                ]
            );

            this.componentSchemasByType = new()
            {
                [typeof(ContentStorageModel)] = this.contentComponentSchema,
                [typeof(EnvironmentStorageModel)] = this.environmentComponentSchema,
                [typeof(ManifestStorageModel)] = this.manifestComponentSchema,
                [typeof(PropertyStorageModel)] = this.propertyComponentSchema,
                [typeof(Texture2DStorageModel)] = this.texture2DComponentSchema,
            };

            this.schemaSerializer = new(Deserializer, Serializer);
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

        #endregion

        private void Write<TMapper, TStorageModel>(ZipArchive zip, string entryName, TMapper mapper, TStorageModel storageModel)
            where TMapper : IMapper
            where TStorageModel : IStorageModel
        {
            ZipArchiveEntry entry = zip.CreateEntry(entryName, CompressionLevel.SmallestSize);
            using Stream stream = entry.Open();

            this.schemaSerializer.Serialize(stream, mapper, storageModel);
        }

        private TStorageModel Read<TStorageModel>(ZipArchive zip, string entryName, ComponentSchema componentSchema, int sourceVersion, int targetVersion)
            where TStorageModel : IStorageModel
        {
            ZipArchiveEntry entry = zip.GetEntry(entryName);

            if (entry == null)
            {
                return default;
            }

            using Stream stream = entry.Open();
            return this.schemaSerializer.Deserialize<TStorageModel>(stream, componentSchema, sourceVersion, targetVersion);
        }

        private static void WriteVersioningHeader(ZipArchive zip)
        {
            ZipArchiveEntry entry = zip.CreateEntry(IOConstants.VERSIONING_HEADER_FILE, CompressionLevel.SmallestSize);
            using Stream stream = entry.Open();

            VersioningHeader versioningHeader = new();
            versioningHeader.SetVersion(IOConstants.SAVE_ENTRY_CONTENT_ID, IOConstants.SAVE_CONTENT_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SAVE_ENTRY_ENVIRONMENT_ID, IOConstants.SAVE_ENVIRONMENT_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SAVE_ENTRY_MANIFEST_ID, IOConstants.SAVE_MANIFEST_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SAVE_ENTRY_PROPERTIES_ID, IOConstants.SAVE_PROPERTIES_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SAVE_ENTRY_THUMBNAIL_ID, IOConstants.SAVE_THUMBNAIL_COMPONENT_VERSION);
            versioningHeader.Serialize(stream);
        }

        private static VersioningHeader ReadVersioningHeader(ZipArchive zip)
        {
            ZipArchiveEntry entry = zip.GetEntry(IOConstants.VERSIONING_HEADER_FILE);

            if (entry == null)
            {
                return null;
            }

            using Stream stream = entry.Open();
            VersioningHeader versioningHeader = new();
            versioningHeader.Deserialize(stream);

            return versioningHeader;
        }

        internal void Save()
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(this.world.Name, IOConstants.SAVE_FILE_EXTENSION));

            using FileStream fs = new(filename, FileMode.Create, FileAccess.Write);
            using ZipArchive zip = new(fs, ZipArchiveMode.Create);

            Write(zip, IOConstants.SAVE_ENTRY_CONTENT_FILE, this.contentMapper, CreateContent());
            Write(zip, IOConstants.SAVE_ENTRY_ENVIRONMENT_FILE, this.environmentMapper, CreateEnvironment());
            Write(zip, IOConstants.SAVE_ENTRY_MANIFEST_FILE, this.manifestMapper, CreateManifest());
            Write(zip, IOConstants.SAVE_ENTRY_PROPERTIES_FILE, this.propertyMapper, CreateProperties());
            Write(zip, IOConstants.SAVE_ENTRY_THUMBNAIL_FILE, this.texture2DMapper, CreateThumbnail());
            WriteVersioningHeader(zip);
        }

        internal TStorageModel Load<TStorageModel>(string name)
            where TStorageModel : IStorageModel
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(name, IOConstants.SAVE_FILE_EXTENSION));

            using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
            using ZipArchive zip = new(fs, ZipArchiveMode.Read);

            Type storageModelType = typeof(TStorageModel);

            VersioningHeader versioningHeader = ReadVersioningHeader(zip);
            ComponentSchema componentSchema = this.componentSchemasByType[storageModelType];
            int targetVersion = this.componentVersionsByType[storageModelType];
            string entryName = this.componentEntryNamesByType[storageModelType];

            _ = versioningHeader.TryGetVersion(string.Empty, out int sourceVersion);

            return Read<TStorageModel>(zip, entryName, componentSchema, sourceVersion, targetVersion);
        }
    }
}

