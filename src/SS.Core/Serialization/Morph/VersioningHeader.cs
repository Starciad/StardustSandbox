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

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StardustSandbox.Core.Serialization.Morph
{
    internal sealed class VersioningHeader
    {
        private readonly Dictionary<string, int> componentVersions = [];
        private readonly Stream stream;

        internal VersioningHeader(Stream stream)
        {
            this.stream = stream;
        }

        internal void SetVersion(string component, int version)
        {
            this.componentVersions[component] = version;
        }

        internal bool TryGetVersion(string component, out int version)
        {
            return this.componentVersions.TryGetValue(component, out version);
        }

        internal void Serialize()
        {
            using BinaryWriter writer = new(this.stream, Encoding.UTF8, leaveOpen: true);

            writer.Write(this.componentVersions.Count);

            foreach (KeyValuePair<string, int> entry in this.componentVersions)
            {
                writer.Write(entry.Key);
                writer.Write(entry.Value);
            }
        }

        internal void Deserialize()
        {
            if (this.stream.CanSeek && this.stream.Position == this.stream.Length)
            {
                return;
            }

            this.componentVersions.Clear();

            using BinaryReader reader = new(this.stream, Encoding.UTF8, leaveOpen: true);

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                int value = reader.ReadInt32();
                this.componentVersions[key] = value;
            }
        }
    }
}
