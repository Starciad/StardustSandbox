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

using System;

namespace StardustSandbox.Core.Enums.Serialization
{
    [Flags]
    internal enum LoadFlags : byte
    {
        None = 0,
        Thumbnail = 1 << 0,
        Metadata = 1 << 1,
        Manifest = 1 << 2,
        Properties = 1 << 3,
        Environment = 1 << 4,
        Content = 1 << 5,

        All = Thumbnail | Metadata | Manifest | Properties | Environment | Content
    }
}

