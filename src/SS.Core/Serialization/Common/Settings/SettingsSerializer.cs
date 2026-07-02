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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.Serialization.Morph;
using StardustSandbox.Core.Serialization.Morph;

using System;
using System.IO;

namespace StardustSandbox.Core.Serialization.Common.Settings
{
    public sealed partial class SettingsSerializer
    {
        private readonly string versioningHeaderFilename = Path.Combine(IO.Directory.Settings, IOConstants.VERSIONING_HEADER_FILE);
        private readonly SchemaSerializer schemaSerializer;

        private void WriteVersioningHeader()
        {
            using FileStream stream = new(this.versioningHeaderFilename, FileMode.Create, FileAccess.Write, FileShare.None);

            VersioningHeader versioningHeader = new();
            versioningHeader.SetVersion(IOConstants.PROGRESS_ACHIEVEMENT_COMPONENT_ID, IOConstants.PROGRESS_ACHIEVEMENT_COMPONENT_VERSION);
            versioningHeader.Serialize(stream);
        }

        private VersioningHeader ReadVersioningHeader()
        {
            using FileStream stream = new(this.versioningHeaderFilename, FileMode.Open, FileAccess.Read, FileShare.Read);

            VersioningHeader versioningHeader = new();
            versioningHeader.Deserialize(stream);

            return versioningHeader;
        }

        private IData Deserializer(Stream stream, Type versionType)
        {

        }

        private void Serializer(Stream stream, IData data)
        {

        }

        internal SettingsSerializer()
        {
            this.schemaSerializer = new(Deserializer, Serializer);

            CreateWarningFile();
        }
        
        private static void CreateWarningFile()
        {
            string filePath = Path.Combine(IO.Directory.Settings, IOConstants.WARNING);

            if (File.Exists(filePath))
            {
                return;
            }

            File.WriteAllText(filePath, builder.ToString());
        }

        internal TStorageModel Load<TStorageModel>() where TStorageModel : IStorageModel
        {

        }

        internal void Save<TStorageModel>(TStorageModel value) where TStorageModel : IStorageModel
        {

        }
    }
}

