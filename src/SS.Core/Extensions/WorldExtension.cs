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
        internal static Texture2D CreateThumbnail(this TileMap tileMap, GraphicsDevice graphicsDevice)
        {
            // Thumbnail dimensions
            int thumbnailWidth = WorldConstants.WORLD_THUMBNAIL_SIZE.X;
            int thumbnailHeight = WorldConstants.WORLD_THUMBNAIL_SIZE.Y;

            // Scale factor for spacing
            float pixelSpacingX = tileMap.Width / (float)thumbnailWidth;
            float pixelSpacingY = tileMap.Height / (float)thumbnailHeight;

            // Create texture for the thumbnail
            Texture2D thumbnailTexture = new(graphicsDevice, thumbnailWidth, thumbnailHeight, false, SurfaceFormat.Color);
            Color[] data = new Color[thumbnailWidth * thumbnailHeight];

            for (int y = 0; y < thumbnailHeight; y++)
            {
                for (int x = 0; x < thumbnailWidth; x++)
                {
                    // Calculate world position from thumbnail position
                    int worldX = (int)(x * pixelSpacingX);
                    int worldY = (int)(y * pixelSpacingY);
                    Point worldPosition = new(worldX, worldY);

                    // Calculate index in the 1D array of the thumbnail
                    int index = (y * thumbnailWidth) + x;

                    // Determines color based on world element
                    if (!tileMap.HasElement(worldPosition, Layer.Foreground) &&
                        !tileMap.HasElement(worldPosition, Layer.Background))
                    {
                        // This color represents the thumbnail's background
                        data[index] = AAP64ColorPalette.Cerulean.Vary(5);
                        continue;
                    }

                    // This color represents the currently selected element
                    if (!tileMap.HasElement(worldPosition, Layer.Foreground))
                    {
                        data[index] = tileMap.GetElement(worldPosition, Layer.Foreground).ReferenceColor.Vary(5);
                        continue;
                    }

                    if (!tileMap.HasElement(worldPosition, Layer.Background))
                    {
                        data[index] = tileMap.GetElement(worldPosition, Layer.Background).ReferenceColor.Vary(5).Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
                        continue;
                    }
                }
            }

            thumbnailTexture.SetData(data);
            return thumbnailTexture;
        }
    }
}
