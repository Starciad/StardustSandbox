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
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.WorldSystem.Components;

namespace StardustSandbox.Core.Extensions
{
    internal static class WorldExtension
    {
        private static Color GetElementColor(TileMap tileMap, Point worldPosition)
        {
            if (tileMap.HasElement(worldPosition, Layer.Foreground))
            {
                return tileMap.GetElement(worldPosition, Layer.Foreground).ReferenceColor.Vary(5);
            }

            return tileMap.GetElement(worldPosition, Layer.Background).ReferenceColor.Vary(5).Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
        }

        private static void SetThumbnailPixelColor(ref Color[] data, TileMap tileMap, Point position, Point thumbnailSize, Point pixelSpacing)
        {
            // Calculate world position from thumbnail position.
            Point worldPosition = new(
                position.X * pixelSpacing.X,
                position.Y * pixelSpacing.Y
            );

            // Calculate index in the 1D array of the thumbnail.
            int index = (position.Y * thumbnailSize.X) + position.X;

            // If there are no elements in either layer at this world position,
            // set the pixel color to a default background color.
            if (!tileMap.HasElement(worldPosition, Layer.Foreground) &&
                !tileMap.HasElement(worldPosition, Layer.Background))
            {
                data[index] = AAP64ColorPalette.Cerulean.Vary(5);
                return;
            }

            data[index] = GetElementColor(tileMap, worldPosition);
        }

        internal static Texture2D CreateThumbnail(this TileMap tileMap, GraphicsDevice graphicsDevice)
        {
            Point thumbnailSize = WorldConstants.WORLD_THUMBNAIL_SIZE;
            Point pixelSpacing = new(tileMap.Width / thumbnailSize.X, tileMap.Height / thumbnailSize.Y);

            Texture2D thumbnailTexture = new(graphicsDevice, thumbnailSize.X, thumbnailSize.Y, false, SurfaceFormat.Color);
            Color[] data = new Color[thumbnailSize.X * thumbnailSize.Y];

            for (int y = 0; y < thumbnailSize.Y; y++)
            {
                for (int x = 0; x < thumbnailSize.X; x++)
                {
                    SetThumbnailPixelColor(ref data, tileMap, new(x, y), thumbnailSize, pixelSpacing);
                }
            }

            thumbnailTexture.SetData(data);
            return thumbnailTexture;
        }
    }
}
