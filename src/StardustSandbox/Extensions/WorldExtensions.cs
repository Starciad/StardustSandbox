using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.World;

using System;

namespace StardustSandbox.Extensions
{
    internal static class WorldExtensions
    {
        internal static Texture2D CreateThumbnail(this GameWorld world, GraphicsDevice graphicsDevice)
        {
            // Thumbnail dimensions
            int thumbnailWidth = WorldConstants.WORLD_THUMBNAIL_SIZE.X;
            int thumbnailHeight = WorldConstants.WORLD_THUMBNAIL_SIZE.Y;

            // Scale factor for spacing
            float pixelSpacingX = Convert.ToSingle(world.Information.Size.X / thumbnailWidth);
            float pixelSpacingY = Convert.ToSingle(world.Information.Size.Y / thumbnailHeight);

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
                    if (world.IsEmptySlot(worldPosition))
                    {
                        // This color represents the thumbnail's background
                        data[index] = AAP64ColorPalette.Cerulean.Vary(5);
                    }
                    else
                    {
                        // This color represents the currently selected element
                        Slot worldSlot = world.GetSlot(worldPosition);

                        if (!worldSlot.Foreground.HasState(ElementStates.IsEmpty))
                        {
                            data[index] = world.GetElement(worldPosition, Layer.Foreground).ReferenceColor.Vary(5);
                        }
                        else if (!worldSlot.Background.HasState(ElementStates.IsEmpty))
                        {
                            data[index] = world.GetElement(worldPosition, Layer.Background).ReferenceColor.Vary(5).Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
                        }
                    }
                }
            }

            thumbnailTexture.SetData(data);
            return thumbnailTexture;
        }
    }
}
