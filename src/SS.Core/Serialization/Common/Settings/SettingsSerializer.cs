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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.Serialization.Common.Settings.Mappers;
using StardustSandbox.Core.Serialization.Common.Settings.StorageModels;
using StardustSandbox.Core.Serialization.Morph;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization.Common.Settings
{
    public sealed partial class SettingsSerializer
    {
        private readonly XmlReaderSettings readerSettings = new()
        {
            DtdProcessing = DtdProcessing.Prohibit
        };

        private readonly XmlWriterSettings writerSettings = new()
        {
            Indent = true,
            Encoding = Encoding.UTF8,
            CloseOutput = false
        };

        #region Mappers

        private readonly ControlMapper controlMapper;
        private readonly CursorMapper cursorMapper;
        private readonly GameplayMapper gameplayMapper;
        private readonly GeneralMapper generalMapper;
        private readonly InterfaceMapper interfaceMapper;
        private readonly VideoMapper videoMapper;
        private readonly VolumeMapper volumeMapper;

        #endregion

        #region Schemas

        private readonly ComponentSchema controlComponentSchema;
        private readonly ComponentSchema cursorComponentSchema;
        private readonly ComponentSchema gameplayComponentSchema;
        private readonly ComponentSchema generalComponentSchema;
        private readonly ComponentSchema interfaceComponentSchema;
        private readonly ComponentSchema videoComponentSchema;
        private readonly ComponentSchema volumeComponentSchema;

        #endregion

        #region Dictionaries

        private readonly Dictionary<Type, string> componentFilenamesByType;
        private readonly Dictionary<Type, ComponentSchema> componentSchemasByType;
        private readonly Dictionary<Type, int> componentVersionsByType;
        private readonly Dictionary<Type, IStorageModel> storageModelCache;

        #endregion

        private readonly string versioningHeaderFilename = Path.Combine(IO.Directory.Settings, IOConstants.VERSIONING_HEADER_FILE);
        private readonly SchemaSerializer schemaSerializer;

        internal SettingsSerializer()
        {
            this.schemaSerializer = new(Deserializer, Serializer);

            #region Mappers

            this.controlMapper = new();
            this.cursorMapper = new();
            this.gameplayMapper = new();
            this.generalMapper = new();
            this.interfaceMapper = new();
            this.videoMapper = new();
            this.volumeMapper = new();

            #endregion

            #region Schemas

            this.controlComponentSchema = new(
                IOConstants.SETTINGS_CONTROL_COMPONENT_ID,
                this.controlMapper,
                [],
                [
                    typeof(Data.V1.ControlData)
                ]
            );

            this.cursorComponentSchema = new(
                IOConstants.SETTINGS_CURSOR_COMPONENT_ID,
                this.cursorMapper,
                [],
                [
                    typeof(Data.V1.CursorData)
                ]
            );

            this.gameplayComponentSchema = new(
                IOConstants.SETTINGS_GAMEPLAY_COMPONENT_ID,
                this.gameplayMapper,
                [],
                [
                    typeof(Data.V1.GameplayData)
                ]
            );

            this.generalComponentSchema = new(
                IOConstants.SETTINGS_GENERAL_COMPONENT_ID,
                this.generalMapper,
                [],
                [
                    typeof(Data.V1.GeneralData)
                ]
            );

            this.interfaceComponentSchema = new(
                IOConstants.SETTINGS_INTERFACE_COMPONENT_ID,
                this.interfaceMapper,
                [],
                [
                    typeof(Data.V1.InterfaceData)
                ]
            );

            this.videoComponentSchema = new(
                IOConstants.SETTINGS_VIDEO_COMPONENT_ID,
                this.videoMapper,
                [],
                [
                    typeof(Data.V1.VideoData)
                ]
            );

            this.volumeComponentSchema = new(
                IOConstants.SETTINGS_VOLUME_COMPONENT_ID,
                this.volumeMapper,
                [],
                [
                    typeof(Data.V1.VolumeData)
                ]
            );

            #endregion

            #region Dictionaries

            this.componentFilenamesByType = new()
            {
                [typeof(ControlStorageModel)] = IOConstants.SETTINGS_CONTROL_COMPONENT_FILE,
                [typeof(CursorStorageModel)] = IOConstants.SETTINGS_CURSOR_COMPONENT_FILE,
                [typeof(GameplayStorageModel)] = IOConstants.SETTINGS_GAMEPLAY_COMPONENT_FILE,
                [typeof(GeneralStorageModel)] = IOConstants.SETTINGS_GENERAL_COMPONENT_FILE,
                [typeof(InterfaceStorageModel)] = IOConstants.SETTINGS_INTERFACE_COMPONENT_FILE,
                [typeof(VideoStorageModel)] = IOConstants.SETTINGS_VIDEO_COMPONENT_FILE,
                [typeof(VolumeStorageModel)] = IOConstants.SETTINGS_VOLUME_COMPONENT_FILE
            };

            this.componentSchemasByType = new()
            {
                [typeof(ControlStorageModel)] = this.controlComponentSchema,
                [typeof(CursorStorageModel)] = this.cursorComponentSchema,
                [typeof(GameplayStorageModel)] = this.gameplayComponentSchema,
                [typeof(GeneralStorageModel)] = this.generalComponentSchema,
                [typeof(InterfaceStorageModel)] = this.interfaceComponentSchema,
                [typeof(VideoStorageModel)] = this.videoComponentSchema,
                [typeof(VolumeStorageModel)] = this.volumeComponentSchema
            };

            this.componentVersionsByType = new()
            {
                [typeof(ControlStorageModel)] = IOConstants.SETTINGS_CONTROL_COMPONENT_VERSION,
                [typeof(CursorStorageModel)] = IOConstants.SETTINGS_CURSOR_COMPONENT_VERSION,
                [typeof(GameplayStorageModel)] = IOConstants.SETTINGS_GAMEPLAY_COMPONENT_VERSION,
                [typeof(GeneralStorageModel)] = IOConstants.SETTINGS_GENERAL_COMPONENT_VERSION,
                [typeof(InterfaceStorageModel)] = IOConstants.SETTINGS_INTERFACE_COMPONENT_VERSION,
                [typeof(VideoStorageModel)] = IOConstants.SETTINGS_VIDEO_COMPONENT_VERSION,
                [typeof(VolumeStorageModel)] = IOConstants.SETTINGS_VOLUME_COMPONENT_VERSION
            };

            this.storageModelCache = [];

            #endregion

            // Ensure the versioning header exists, if not, delete component
            // files and create a new versioning header.
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

        private void SaveVersioningHeader()
        {
            using FileStream stream = new(this.versioningHeaderFilename, FileMode.Create, FileAccess.Write, FileShare.None);
            VersioningHeader versioningHeader = new();
            versioningHeader.SetVersion(IOConstants.SETTINGS_CONTROL_COMPONENT_ID, IOConstants.SETTINGS_CONTROL_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SETTINGS_CURSOR_COMPONENT_ID, IOConstants.SETTINGS_CURSOR_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SETTINGS_GAMEPLAY_COMPONENT_ID, IOConstants.SETTINGS_GAMEPLAY_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SETTINGS_GENERAL_COMPONENT_ID, IOConstants.SETTINGS_GENERAL_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SETTINGS_INTERFACE_COMPONENT_ID, IOConstants.SETTINGS_INTERFACE_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SETTINGS_VIDEO_COMPONENT_ID, IOConstants.SETTINGS_VIDEO_COMPONENT_VERSION);
            versioningHeader.SetVersion(IOConstants.SETTINGS_VOLUME_COMPONENT_ID, IOConstants.SETTINGS_VOLUME_COMPONENT_VERSION);
            versioningHeader.Serialize(stream);
        }

        private void UpdateVersioningHeader(VersioningHeader versioningHeader)
        {
            using FileStream stream = new(this.versioningHeaderFilename, FileMode.Create, FileAccess.Write, FileShare.None);
            versioningHeader.Serialize(stream);
        }

        private VersioningHeader LoadVersioningHeader()
        {
            using FileStream stream = new(this.versioningHeaderFilename, FileMode.Open, FileAccess.Read, FileShare.Read);

            VersioningHeader versioningHeader = new();
            versioningHeader.Deserialize(stream);

            return versioningHeader;
        }

        private void DeleteComponents()
        {
            foreach (string filename in this.componentFilenamesByType.Values)
            {
                File.Delete(Path.Combine(IO.Directory.Settings, filename));
            }
        }

        private void InitializeComponents()
        {
            Initialize<ControlStorageModel>();
            Initialize<CursorStorageModel>();
            Initialize<GameplayStorageModel>();
            Initialize<GeneralStorageModel>();
            Initialize<InterfaceStorageModel>();
            Initialize<VideoStorageModel>();
            Initialize<VolumeStorageModel>();
        }

        #endregion

        #region Serialization

        private IData Deserializer(Stream stream, Type versionType)
        {
            using XmlReader reader = XmlReader.Create(stream, this.readerSettings);

            XmlSerializer serializer = new(versionType);
            return (IData)serializer.Deserialize(reader);
        }

        private void Serializer(Stream stream, IData data)
        {
            using XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);

            XmlSerializer serializer = new(data.GetType());
            serializer.Serialize(writer, data);
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

            string componentFilename = Path.Combine(IO.Directory.Settings, this.componentFilenamesByType[storageModelType]);
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

            string filename = Path.Combine(IO.Directory.Settings, this.componentFilenamesByType[storageModelType]);
            using FileStream stream = new(filename, FileMode.Create, FileAccess.Write, FileShare.None);

            this.schemaSerializer.Serialize(stream, schema.Mapper, value);
        }
    }
}
