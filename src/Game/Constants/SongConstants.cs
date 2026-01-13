/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Enums.Assets;

namespace StardustSandbox.Constants
{
    internal static class SongConstants
    {
        internal const float FADE_STEP_INTERVAL_MS = 50.0f;
        internal const float FADE_DURATION_MS = 1500.0f;

        internal static readonly SongIndex[] GAMEPLAY_SONGS =
        [
            SongIndex.Volume_01_Track_03,
            SongIndex.Volume_01_Track_04,
            SongIndex.Volume_01_Track_05,
            SongIndex.Volume_01_Track_06,
        ];
    }
}

