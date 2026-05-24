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

using StardustSandbox.Core.Interfaces.Serialization;

using System;
using System.IO;
using System.Xml.Serialization;

namespace StardustSandbox.Core.Serialization
{
    public sealed partial class SettingsSerializer
    {
        private interface ISettingsDescriptor
        {
            Type SettingsType { get; }
            void Load();
        }

        private sealed class SettingsDescriptor<T>(string fileName) : ISettingsDescriptor where T : ISettingsModule, new()
        {
            public Type SettingsType => typeof(T);
            public T Value => this.cache;

            private T cache;
            private readonly XmlSerializer serializer = new(typeof(T));

            public void Load()
            {
                string filePath = Path.Combine(IO.Directory.Settings, fileName);

                if (!File.Exists(filePath))
                {
                    CreateAndSaveDefault(filePath);
                    return;
                }

                try
                {
                    using FileStream stream = File.OpenRead(filePath);
                    this.cache = (T)this.serializer.Deserialize(stream);
                }
                catch
                {
                    File.Delete(filePath);
                    CreateAndSaveDefault(filePath);
                }
            }

            public void Save(T value)
            {
                using FileStream stream = new(Path.Combine(IO.Directory.Settings, fileName), FileMode.Create, FileAccess.Write);

                this.cache = value;
                this.serializer.Serialize(stream, value);
            }

            private void CreateAndSaveDefault(string filePath)
            {
                this.cache = new T();

                using FileStream stream = new(filePath, FileMode.Create, FileAccess.Write);

                this.serializer.Serialize(stream, this.cache);
            }
        }
    }
}
