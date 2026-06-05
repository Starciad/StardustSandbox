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

using StardustSandbox.Core.Interfaces.Serialization.Worlds;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Serialization.Worlds.Formats.V1
{
    [Serializable]
    [MessagePackObject]
    public sealed class ManifestData : IData
    {
        [Key(0)]
        public bool IsInitialized { get; set; }

        [Key(1)]
        public int SaveVersion { get; set; }

        [Key(2)]
        public Dictionary<string, int> ComponentVersions { get; set; }

        [Key(3)]
        public Version GameVersion { get; set; }

        [Key(4)]
        public string Name { get; set; }

        [Key(5)]
        public string Description { get; set; }

        [Key(6)]
        public DateTime CreationTimestamp { get; set; }

        [Key(7)]
        public DateTime LastModifiedTimestamp { get; set; }
    }
}

