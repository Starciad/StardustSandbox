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
            .WithResolver(StandardResolver.Instance)
            .WithSecurity(MessagePackSecurity.UntrustedData)
            .WithCompression(MessagePackCompression.Lz4BlockArray);

        // Mappers
        private readonly ActorMapper actorMapper;
        private readonly ContentMapper contentMapper;
        private readonly EnvironmentMapper environmentMapper;
        private readonly ManifestMapper manifestMapper;
        private readonly PropertyMapper propertyMapper;
        private readonly SlotLayerMapper slotLayerMapper;
        private readonly SlotMapper slotMapper;
        private readonly Texture2DMapper texture2DMapper;

        // Schemas
        private readonly ComponentSchema actorComponentSchema;
        private readonly ComponentSchema contentComponentSchema;
        private readonly ComponentSchema environmentComponentSchema;
        private readonly ComponentSchema manifestComponentSchema;
        private readonly ComponentSchema propertyComponentSchema;
        private readonly ComponentSchema slotLayerComponentSchema;
        private readonly ComponentSchema slotComponentSchema;
        private readonly ComponentSchema texture2DComponentSchema;

        // Dictionaries
        private readonly Dictionary<Type, ComponentSchema> componentSchemasByType;
        private readonly Dictionary<Type, int> componentVersionsByType = new()
        {
            [typeof(ContentStorageModel)] = IOConstants.WORLD_CONTENT_COMPONENT_VERSION,
            [typeof(EnvironmentStorageModel)] = IOConstants.WORLD_ENVIRONMENT_COMPONENT_VERSION,
            [typeof(ManifestStorageModel)] = IOConstants.WORLD_MANIFEST_COMPONENT_VERSION,
            [typeof(PropertyStorageModel)] = IOConstants.WORLD_PROPERTIES_COMPONENT_VERSION,
            [typeof(Texture2DStorageModel)] = IOConstants.WORLD_THUMBNAIL_COMPONENT_VERSION,
        };
        private readonly Dictionary<Type, string> componentEntryNamesByType = new()
        {
            [typeof(ContentStorageModel)] = IOConstants.WORLD_CONTENT_COMPONENT_FILE,
            [typeof(EnvironmentStorageModel)] = IOConstants.WORLD_ENVIRONMENT_COMPONENT_FILE,
            [typeof(ManifestStorageModel)] = IOConstants.WORLD_MANIFEST_COMPONENT_FILE,
            [typeof(PropertyStorageModel)] = IOConstants.WORLD_PROPERTIES_COMPONENT_FILE,
            [typeof(Texture2DStorageModel)] = IOConstants.WORLD_THUMBNAIL_COMPONENT_FILE,
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
            this.schemaSerializer = new(Deserializer, Serializer);

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
                string.Empty,
                this.actorMapper,
                [],
                [
                    typeof(Data.V1.ActorData)
                ]
            );

            this.contentComponentSchema = new(
                IOConstants.WORLD_CONTENT_COMPONENT_ID,
                this.contentMapper,
                [],
                [
                    typeof(Data.V1.ContentData)
                ]
            );

            this.environmentComponentSchema = new(
                IOConstants.WORLD_ENVIRONMENT_COMPONENT_ID,
                this.environmentMapper,
                [],
                [
                    typeof(Data.V1.EnvironmentData)
                ]
            );

             this.manifestComponentSchema = new(
                 IOConstants.WORLD_MANIFEST_COMPONENT_ID,
                 this.manifestMapper,
                 [],
                 [
                     typeof(Data.V1.ManifestData)
                 ]
             );

            this.propertyComponentSchema = new(
                IOConstants.WORLD_PROPERTIES_COMPONENT_ID,
                this.propertyMapper,
                [],
                [
                    typeof(Data.V1.PropertyData)
                ]
            );

            this.slotLayerComponentSchema = new(
                string.Empty,
                this.slotLayerMapper,
                [],
                [
                    typeof(Data.V1.SlotLayerData)
                ]
            );

            this.slotComponentSchema = new(

                string.Empty,
                this.slotMapper,
                [],
                [
                    typeof(Data.V1.SlotData)
                ]
            );

            this.texture2DComponentSchema = new(
                IOConstants.WORLD_THUMBNAIL_COMPONENT_ID,
                this.texture2DMapper,
                [],
                [
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

        private void Write<TMapper, TStorageModel>(ZipArchive zip, string entryName, TMapper mapper, TStorageModel value)
            where TMapper : IMapper
            where TStorageModel : IStorageModel
        {
            ZipArchiveEntry entry = zip.CreateEntry(entryName, CompressionLevel.SmallestSize);
            using Stream stream = entry.Open();

            this.schemaSerializer.Serialize(stream, mapper, value);
        }

        private TStorageModel Read<TStorageModel>(ZipArchive zip, string entryName, ComponentSchema componentSchema, int sourceVersion, int targetVersion) where TStorageModel : IStorageModel
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
            versioningHeader.SetVersion(IOConstants.WORLD_CONTENT_COMPONENT_ID, IOConstants.WORLD_CONTENT_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.WORLD_ENVIRONMENT_COMPONENT_ID, IOConstants.WORLD_ENVIRONMENT_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.WORLD_MANIFEST_COMPONENT_ID, IOConstants.WORLD_MANIFEST_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.WORLD_PROPERTIES_COMPONENT_ID, IOConstants.WORLD_PROPERTIES_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.WORLD_THUMBNAIL_COMPONENT_ID, IOConstants.WORLD_THUMBNAIL_COMPONENT_VERSION);
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

        internal TStorageModel Load<TStorageModel>(string name) where TStorageModel : IStorageModel
        {
            Type storageModelType = typeof(TStorageModel);

            int targetVersion = this.componentVersionsByType[storageModelType];
            string entryName = this.componentEntryNamesByType[storageModelType];
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(name, IOConstants.WORLD_FILE_EXTENSION));

            using FileStream fs = new(filename, FileMode.Open, FileAccess.Read);
            using ZipArchive zip = new(fs, ZipArchiveMode.Read);

            ComponentSchema schema = this.componentSchemasByType[storageModelType];
            VersioningHeader versioningHeader = ReadVersioningHeader(zip);

            if (!versioningHeader.TryGetVersion(schema.Identifier, out int sourceVersion))
            {
                sourceVersion = targetVersion;
            }

            return Read<TStorageModel>(zip, entryName, schema, sourceVersion, targetVersion);
        }

        internal void Save()
        {
            string filename = Path.Combine(IO.Directory.Worlds, string.Concat(this.world.Name, IOConstants.WORLD_FILE_EXTENSION));

            using FileStream fs = new(filename, FileMode.Create, FileAccess.Write);
            using ZipArchive zip = new(fs, ZipArchiveMode.Create);

            Write(zip, IOConstants.WORLD_CONTENT_COMPONENT_FILE, this.contentMapper, CreateContent());
            Write(zip, IOConstants.WORLD_ENVIRONMENT_COMPONENT_FILE, this.environmentMapper, CreateEnvironment());
            Write(zip, IOConstants.WORLD_MANIFEST_COMPONENT_FILE, this.manifestMapper, CreateManifest());
            Write(zip, IOConstants.WORLD_PROPERTIES_COMPONENT_FILE, this.propertyMapper, CreateProperties());
            Write(zip, IOConstants.WORLD_THUMBNAIL_COMPONENT_FILE, this.texture2DMapper, CreateThumbnail());
            WriteVersioningHeader(zip);
        }
    }
}

