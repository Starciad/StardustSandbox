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

using Microsoft.Xna.Framework;

using StardustSandbox.Backgrounds;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Backgrounds;

using System;

namespace StardustSandbox.Databases
{
    internal static class BackgroundDatabase
    {
        private static Background[] backgrounds;

        private static bool isLoaded = false;

        internal static void Load()
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(BackgroundDatabase)} has already been loaded.");
            }

            backgrounds = [
                // [0] Main Menu
                new([
                    new(new(2.0f, 0.0f), new(-16.0f, 0.0f), false, true),
                ], true, AssetDatabase.GetTexture(TextureIndex.BackgroundOcean)),

                // [1] Ocean
                new([
                    new(new(2.0f, 0.0f), Vector2.Zero, false, true),
                ], true, AssetDatabase.GetTexture(TextureIndex.BackgroundOcean)),

                // [2] Credits
                new([
                    new(new(0.0f, 0.0f), new(-32.0f), false, false),
                ], false, AssetDatabase.GetTexture(TextureIndex.PatternDiamonds)),
            ];

            isLoaded = true;
        }

        internal static Background GetBackground(in BackgroundIndex index)
        {
            return backgrounds[(int)index];
        }
    }
}

