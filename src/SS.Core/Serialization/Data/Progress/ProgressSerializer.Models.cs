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

using StardustSandbox.Core.Interfaces.Serialization.Migrations;
using StardustSandbox.Core.Serialization.Data;
using StardustSandbox.Core.Serialization.Migrations;

using System;
using System.IO;

namespace StardustSandbox.Core.Serialization
{
    internal sealed partial class ProgressSerializer
    {
        private interface IProgressDescriptor
        {
            Type SettingsType { get; }
            void Load(int sourceVersion, int targetVersion);
        }

        private sealed class ProgressDescriptor<TStorageModel>(string filename, DataSerializer dataSerializer, IMapper mapper, MigrationRegistry migrationRegistry) : IProgressDescriptor where TStorageModel : IStorageModel, new()
        {
            public Type SettingsType => typeof(TStorageModel);
            public TStorageModel Value => this.cache;

            private TStorageModel cache;

            public void Load(int sourceVersion, int targetVersion)
            {
                string filePath = Path.Combine(IO.Directory.Progress, filename);

                if (!File.Exists(filePath))
                {
                    CreateAndSaveDefault(filePath);
                    return;
                }

                try
                {
                    using FileStream stream = File.OpenRead(filePath);
                    this.cache = dataSerializer.Deserialize<TStorageModel>(stream, mapper, migrationRegistry, sourceVersion, targetVersion);
                }
                catch
                {
                    File.Delete(filePath);
                    CreateAndSaveDefault(filePath);
                }
            }

            public void Save(TStorageModel value)
            {
                using FileStream stream = new(Path.Combine(IO.Directory.Progress, filename), FileMode.Create, FileAccess.Write);

                this.cache = value;
                dataSerializer.Serialize(stream, mapper, value);
            }

            private void CreateAndSaveDefault(string filePath)
            {
                this.cache = new TStorageModel();

                using FileStream stream = new(filePath, FileMode.Create, FileAccess.Write);
                MessagePackSerializer.Serialize(stream, this.cache);
            }
        }
    }
}
