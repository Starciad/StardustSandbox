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
using StardustSandbox.Core.Serialization.Common.Progress.Mappers;
using StardustSandbox.Core.Serialization.Common.Progress.StorageModels;
using StardustSandbox.Core.Serialization.Morph;

using System;
using System.Collections.Generic;
using System.IO;

namespace StardustSandbox.Core.Serialization
{
    internal sealed partial class ProgressSerializer
    {
        private readonly string versioningHeaderFilename = Path.Combine(IO.Directory.Progress, IOConstants.VERSIONING_HEADER_FILE);

        private readonly SchemaSerializer schemaSerializer;
        private readonly Dictionary<Type, IProgressDescriptor> descriptors;

        private VersioningHeader WriteVersioningHeader()
        {
            using FileStream fs = new(versioningHeaderFilename, FileMode.Create, FileAccess.Write);
            VersioningHeader versioningHeader = new();
            versioningHeader.SetVersion(IOConstants.ACHIEVEMENT_PROGRESS_FILE, IOConstants.ACHIEVEMENT_PROGRESS_VERSION);
            versioningHeader.Serialize(fs);

            return versioningHeader;
        }

        private VersioningHeader ReadVersioningHeader()
        {
            if (!File.Exists(this.versioningHeaderFilename))
            {
                return WriteVersioningHeader();
            }

            using FileStream fs = new(versioningHeaderFilename, FileMode.Open, FileAccess.Read);
            VersioningHeader versioningHeader = new();
            versioningHeader.Deserialize(fs);
            return versioningHeader;
        }

        internal ProgressSerializer(SchemaSerializer schemaSerializer)
        {
            this.schemaSerializer = schemaSerializer;

            this.descriptors = new()
            {
                [typeof(AchievementStorageModel)] = new ProgressDescriptor<AchievementStorageModel>(IOConstants.ACHIEVEMENT_PROGRESS_FILE, schemaSerializer, new AchievementMapper(), new()),
            };

            _ = Directory.CreateDirectory(IO.Directory.Progress);
            VersioningHeader versioningHeader = ReadVersioningHeader();

            foreach (IProgressDescriptor descriptor in this.descriptors.Values)
            {
                descriptor.Load();
            }
        }

        public T Load<T>() where T : IStorageModel, new()
        {
            return GetDescriptor<T>().Value;
        }

        public void Save<T>(T value) where T : IStorageModel, new()
        {
            GetDescriptor<T>().Save(value);
        }

        private ProgressDescriptor<TStorageModel> GetDescriptor<TStorageModel>() where TStorageModel : IStorageModel, new()
        {
            return !this.descriptors.TryGetValue(typeof(TStorageModel), out IProgressDescriptor raw) ? null : (ProgressDescriptor<TStorageModel>)raw;
        }
    }
}
