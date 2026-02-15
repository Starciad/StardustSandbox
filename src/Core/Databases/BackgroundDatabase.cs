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

using StardustSandbox.Core.Backgrounds;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Backgrounds;

using System;

namespace StardustSandbox.Core.Databases
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
                new()
                {
                    IsAffectedByLighting = true,
                    Layers = [
                        // Clouds
                        new()
                        {
                            Anchoring = BackgroundAnchoring.South,
                            AnchoringOffset = new(0.0f, -338.0f),
                            AutoMovementSpeed = new(-6.0f, 0.0f),
                            IsFixedVertically = true,
                            RepeatHorizontally = true,
                            Texture = AssetDatabase.GetTexture(TextureIndex.BackgroundClouds),
                            TextureSourceRectangle = new(0, 0, 1280, 240),
                        },

                        // Ocean
                        new()
                        {
                            Anchoring = BackgroundAnchoring.South,
                            AnchoringOffset = new(0.0f, -184.0f),
                            AutoMovementSpeed = new(-16.0f, 0.0f),
                            IsFixedVertically = true,
                            RepeatHorizontally = true,
                            Texture = AssetDatabase.GetTexture(TextureIndex.BackgroundOcean),
                            TextureSourceRectangle = new(0, 0, 1280, 216),
                        },
                    ],
                },

                // [1] Ocean
                new()
                {
                    IsAffectedByLighting = true,
                    Layers = [
                        // Clouds
                        new()
                        {
                            Anchoring = BackgroundAnchoring.South,
                            AnchoringOffset = new(0.0f, -338.0f),
                            AutoMovementSpeed = new(-3.0f, 0.0f),
                            IsFixedVertically = true,
                            ParallaxSpeed = new(0.008f, 0.0f),
                            RepeatHorizontally = true,
                            Texture = AssetDatabase.GetTexture(TextureIndex.BackgroundClouds),
                            TextureSourceRectangle = new(0, 0, 1280, 240),
                        },

                        // Ocean
                        new()
                        {
                            Anchoring = BackgroundAnchoring.South,
                            AnchoringOffset = new(0.0f, -184.0f),
                            IsFixedVertically = true,
                            ParallaxSpeed = new(0.01f, 0.0f),
                            RepeatHorizontally = true,
                            Texture = AssetDatabase.GetTexture(TextureIndex.BackgroundOcean),
                            TextureSourceRectangle = new(0, 0, 1280, 216),
                        },
                    ],
                },

                // [2] Credits
                new()
                {
                    Layers = [
                        new()
                        {
                            Anchoring = BackgroundAnchoring.Northwest,
                            AutoMovementSpeed = new(-32.0f),
                            RepeatHorizontally = true,
                            RepeatVertically = true,
                            Texture = AssetDatabase.GetTexture(TextureIndex.PatternDiamonds),
                            TextureSourceRectangle = new(0, 0, 80, 80),
                        }
                    ],
                },
            ];

            isLoaded = true;
        }

        internal static Background GetBackground(in BackgroundIndex index)
        {
            return backgrounds[(int)index];
        }
    }
}
