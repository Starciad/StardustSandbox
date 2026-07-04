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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.Serialization.Common.Progress.Mappers;
using StardustSandbox.Core.Serialization.Common.Progress.StorageModels;
using StardustSandbox.Core.Serialization.Morph;

using System;
using System.Collections.Generic;
using System.IO;

namespace StardustSandbox.Core.Serialization.Common.Progress
{
    internal sealed partial class ProgressSerializer
    {
        private readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard
            .WithResolver(StandardResolver.Instance)
            .WithSecurity(MessagePackSecurity.UntrustedData)
            .WithCompression(MessagePackCompression.Lz4BlockArray);

        #region Mappers

        private readonly AchievementMapper achievementMapper;

        #endregion

        #region Schemas

        private readonly ComponentSchema achievementComponentSchema;

        #endregion

        #region Dictionaries

        private readonly Dictionary<Type, string> componentFilenamesByType;
        private readonly Dictionary<Type, ComponentSchema> componentSchemasByType;
        private readonly Dictionary<Type, int> componentVersionsByType;
        private readonly Dictionary<Type, IStorageModel> storageModelCache;

        #endregion

        private readonly string versioningHeaderFilename = Path.Combine(IO.Directory.Progress, IOConstants.VERSIONING_HEADER_FILE);
        private readonly SchemaSerializer schemaSerializer;
        
        internal ProgressSerializer()
        {
            this.schemaSerializer = new(Deserializer, Serializer);

            #region Mappers

            this.achievementMapper = new();

            #endregion

            #region Schemas

            this.achievementComponentSchema = new(
                IOConstants.PROGRESS_ACHIEVEMENT_COMPONENT_ID,
                this.achievementMapper,
                [],
                [
                    typeof(Data.V1.AchievementData)
                ]
            );

            #endregion

            #region Dictionaries

            this.componentFilenamesByType = new()
            {
                [typeof(AchievementStorageModel)] = IOConstants.PROGRESS_ACHIEVEMENT_COMPONENT_FILE,
            };

            this.componentSchemasByType = new()
            {
                [typeof(AchievementStorageModel)] = this.achievementComponentSchema
            };

            this.componentVersionsByType = new()
            {
                [typeof(AchievementStorageModel)] = IOConstants.PROGRESS_ACHIEVEMENT_COMPONENT_VERSION,
            };

            this.storageModelCache = [];

            #endregion

            // Check if the versioning header exists, if not, delete all
            // component files and save a new versioning header.
            if (!VersioningHeaderExists())
            {
                DeleteComponents();
                InitializeComponents();
                SaveVersioningHeader();
            }
        }

        #region Versioning Header

        private bool VersioningHeaderExists()
        {
            return File.Exists(this.versioningHeaderFilename);
        }

        private VersioningHeader LoadVersioningHeader()
        {
            using FileStream stream = new(this.versioningHeaderFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
            VersioningHeader versioningHeader = new();
            versioningHeader.Deserialize(stream);
            return versioningHeader;
        }

        private void SaveVersioningHeader()
        {
            using FileStream stream = new(this.versioningHeaderFilename, FileMode.Create, FileAccess.Write, FileShare.None);
            VersioningHeader versioningHeader = new();
            versioningHeader.SetVersion(IOConstants.PROGRESS_ACHIEVEMENT_COMPONENT_ID, IOConstants.PROGRESS_ACHIEVEMENT_COMPONENT_VERSION);
            versioningHeader.Serialize(stream);
        }

        private void UpdateVersioningHeader(VersioningHeader versioningHeader)
        {
            using FileStream stream = new(this.versioningHeaderFilename, FileMode.Create, FileAccess.Write, FileShare.None);
            versioningHeader.Serialize(stream);
        }

        private void DeleteComponents()
        {
            foreach (string filename in this.componentFilenamesByType.Values)
            {
                File.Delete(Path.Combine(IO.Directory.Progress, filename));
            }
        }

        private void InitializeComponents()
        {
            Initialize<AchievementStorageModel>();
        }

        #endregion

        #region Serialization

        private IData Deserializer(Stream stream, Type versionType)
        {
            return (IData)MessagePackSerializer.Deserialize(versionType, stream, this.options);
        }

        private void Serializer(Stream stream, IData data)
        {
            MessagePackSerializer.Serialize(data.GetType(), stream, data, this.options);
        }

        #endregion

        private void Initialize<TStorageModel>() where TStorageModel : IStorageModel, new()
        {
            Type storageModelType = typeof(TStorageModel);

            if (this.storageModelCache.ContainsKey(storageModelType))
            {
                return;
            }

            TStorageModel newModel = new();
            this.storageModelCache[storageModelType] = newModel;
            Save(newModel);
        }

        internal TStorageModel Load<TStorageModel>() where TStorageModel : IStorageModel
        {
            Type storageModelType = typeof(TStorageModel);

            if (this.storageModelCache.TryGetValue(storageModelType, out IStorageModel cachedModel))
            {
                return (TStorageModel)cachedModel;
            }

            string componentFilename = Path.Combine(IO.Directory.Progress, this.componentFilenamesByType[storageModelType]);
            int targetVersion = this.componentVersionsByType[storageModelType];

            ComponentSchema schema = this.componentSchemasByType[storageModelType];
            VersioningHeader versioningHeader = LoadVersioningHeader();

            if (!versioningHeader.TryGetVersion(schema.Identifier, out int sourceVersion))
            {
                sourceVersion = targetVersion;
                versioningHeader.SetVersion(schema.Identifier, sourceVersion);
                UpdateVersioningHeader(versioningHeader);
            }

            using FileStream stream = new(componentFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
            TStorageModel loadedModel = this.schemaSerializer.Deserialize<TStorageModel>(stream, schema, sourceVersion, targetVersion);
            this.storageModelCache[storageModelType] = loadedModel;

            return loadedModel;
        }

        internal void Save<TStorageModel>(TStorageModel value) where TStorageModel : IStorageModel
        {
            Type storageModelType = typeof(TStorageModel);

            this.storageModelCache[storageModelType] = value;

            ComponentSchema schema = this.componentSchemasByType[storageModelType];

            string filename = Path.Combine(IO.Directory.Progress, this.componentFilenamesByType[storageModelType]);
            using FileStream stream = new(filename, FileMode.Create, FileAccess.Write, FileShare.None);

            this.schemaSerializer.Serialize(stream, schema.Mapper, value);
        }
    }
}
