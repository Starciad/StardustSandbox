using Microsoft.Xna.Framework;

using SkiaSharp;

using StardustSandbox.Constants;
using StardustSandbox.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.IO;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Screenshot
{
    internal static class WorldScreenshot
    {
        private static ElementContext elementScreenshotContext;

        private static void DrawSlotLayer(SKCanvas canvas, in Point position, in Layer layer, Slot slot, Element element)
        {
            elementScreenshotContext.UpdateInformation(position, layer, slot);

            ElementScreenshot.Capture(elementScreenshotContext, element, canvas, element.TextureOriginOffset.ToSKPointI());
        }

        internal static void Capture(World world)
        {
            elementScreenshotContext ??= new(world);

            int pixelWidth = world.Information.Size.X * WorldConstants.GRID_SIZE;
            int pixelHeight = world.Information.Size.Y * WorldConstants.GRID_SIZE;

            using SKBitmap bitmap = new(pixelWidth, pixelHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
            using SKCanvas canvas = new(bitmap);

            canvas.Clear(SKColors.Transparent);

            for (int y = 0; y < world.Information.Size.Y; y++)
            {
                for (int x = 0; x < world.Information.Size.X; x++)
                {
                    if (!world.TryGetSlot(new(x, y), out Slot foundSlot))
                    {
                        continue;
                    }

                    SlotLayer backgroundLayer = foundSlot.GetLayer(Layer.Background);
                    SlotLayer foregroundLayer = foundSlot.GetLayer(Layer.Foreground);

                    // Background layer
                    if (!backgroundLayer.IsEmpty)
                    {
                        Element backgroundElement = backgroundLayer.Element;
                        DrawSlotLayer(canvas, foundSlot.Position, Layer.Background, foundSlot, backgroundElement);
                    }

                    // Foreground layer
                    if (!foregroundLayer.IsEmpty)
                    {
                        Element foregroundElement = foregroundLayer.Element;
                        DrawSlotLayer(canvas, foundSlot.Position, Layer.Foreground, foundSlot, foregroundElement);
                    }
                }
            }

            SSFile.WriteWorldScreenshot(SKImage.FromBitmap(bitmap));
        }
    }
}
