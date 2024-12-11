﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World;

namespace StardustSandbox.Core.Extensions
{
    public static class SWorldExtensions
    {
        public static Texture2D CreateThumbnail(this SWorld world, GraphicsDevice graphicsDevice)
        {
            // Thumbnail dimensions
            int thumbnailWidth = SWorldConstants.WORLD_THUMBNAIL_SIZE.Width;
            int thumbnailHeight = SWorldConstants.WORLD_THUMBNAIL_SIZE.Height;

            // Dimensions of the world
            SSize2 worldSize = world.Infos.Size;
            int worldWidth = worldSize.Width;
            int worldHeight = worldSize.Height;

            // Scale factor for spacing
            float pixelSpacingX = (float)worldWidth / thumbnailWidth;
            float pixelSpacingY = (float)worldHeight / thumbnailHeight;

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
                    if (world.IsEmptyElementSlot(worldPosition))
                    {
                        // This color represents the thumbnail's background
                        data[index] = SColorPalette.Cerulean.Vary(5);
                    }
                    else
                    {
                        // This color represents the currently selected element
                        data[index] = world.GetElement(worldPosition).ReferenceColor.Vary(5);
                    }
                }
            }

            thumbnailTexture.SetData(data);
            return thumbnailTexture;
        }
    }
}