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

using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.Constants
{
    internal static class ScreenConstants
    {
        // 16:9 Aspect Ratio
        internal static Point[] RESOLUTIONS =>
        [
            new(854, 480), // [00] - FWVGA
            new(960, 540), // [01] - qHD
            new(1280, 720), // [02] - SD / HD ready (720p)
            new(1366, 768), // [03] - WXGA
            new(1600, 900), // [04] - HD+
            new(1920, 1080), // [05] - FHD / Full HD (1080p)
        ];
    }
}
