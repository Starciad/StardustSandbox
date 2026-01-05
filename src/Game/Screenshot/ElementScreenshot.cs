using Microsoft.Xna.Framework;

using SkiaSharp;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Screenshot
{
    internal static class ElementScreenshot
    {
        private readonly struct BlobInfo(Point position, byte blobValue)
        {
            internal readonly Point Position => position;
            internal readonly byte BlobValue => blobValue;
        }

        private static SKImage elementsImage;

        private static readonly BlobInfo[] blobInfos = new BlobInfo[3];
        private static readonly SKPoint[] spritePositions = new SKPoint[ElementConstants.SPRITE_DIVISIONS_LENGTH];
        private static readonly SKRectI[] spriteClipAreas = new SKRectI[ElementConstants.SPRITE_DIVISIONS_LENGTH];

        // sprite key points (20 slices), positions computed as [left, top, right, bottom]
        private static readonly SKRectI[] spriteKeyPoints =
        [
            // Full 0,1,2,3 (NW, NE, SW, SE)
            new(0, 0, ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(16, 0, 16 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(0, 16, ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),
            new(16, 16, 16 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),

            // Corner 4,5,6,7
            new(32, 0, 32 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(48, 0, 48 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(32, 16, 32 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),
            new(48, 16, 48 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),

            // Vertical Edge 8,9,10,11
            new(64, 0, 64 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(80, 0, 80 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(64, 16, 64 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),
            new(80, 16, 80 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),

            // Horizontal Border 12,13,14,15
            new(96, 0, 96 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(112, 0, 112 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(96, 16, 96 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),
            new(112, 16, 112 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),

            // Gaps 16,17,18,19
            new(128, 0, 128 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(144, 0, 144 + ElementConstants.SPRITE_SLICE_SIZE, ElementConstants.SPRITE_SLICE_SIZE),
            new(128, 16, 128 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE),
            new(144, 16, 144 + ElementConstants.SPRITE_SLICE_SIZE, 16 + ElementConstants.SPRITE_SLICE_SIZE)
        ];

        internal static void Load()
        {
            elementsImage = AssetDatabase.GetTexture(TextureIndex.Elements).ToSKImage();
        }

        internal static void Unload()
        {
            elementsImage.Dispose();
        }

        // ====== BLOB HELPERS ======
        private static void UpdateSpritePositions(in Point position)
        {
            float xOffset = ElementConstants.SPRITE_X_OFFSET;
            float yOffset = ElementConstants.SPRITE_Y_OFFSET;

            float floatX = (float)position.X * WorldConstants.GRID_SIZE;
            float floatY = (float)position.Y * WorldConstants.GRID_SIZE;

            spritePositions[0] = new(floatX, floatY);
            spritePositions[1] = new((position.X + xOffset) * WorldConstants.GRID_SIZE, floatY);
            spritePositions[2] = new(floatX, (position.Y + yOffset) * WorldConstants.GRID_SIZE);
            spritePositions[3] = new((position.X + xOffset) * WorldConstants.GRID_SIZE, (position.Y + yOffset) * WorldConstants.GRID_SIZE);
        }

        private static void UpdateSpriteSlice(ElementContext context, in ElementIndex elementIndex, in int index, in Point position)
        {
            byte blobValue = GetBlobValueFromTargetPositions(context, elementIndex, index, position);
            SetChunkSpriteFromIndexAndBlobValue(index, blobValue);
        }

        private static byte GetBlobValueFromTargetPositions(ElementContext context, in ElementIndex elementIndex, in int index, in Point position)
        {
            byte result = 0;

            GetTargetPositionsFromIndex(index, position);

            for (int i = 0; i < blobInfos.Length; i++)
            {
                Point targetPos = blobInfos[i].Position;

                bool found = context.TryGetElement(targetPos, context.Layer, out ElementIndex targetElement);

                if (found == false)
                {
                    continue;
                }

                if (targetElement != elementIndex)
                {
                    continue;
                }

                result = (byte)(result + blobInfos[i].BlobValue);
            }

            return result;
        }

        private static void GetTargetPositionsFromIndex(in int index, in Point position)
        {
            switch (index)
            {
                case 0:
                    blobInfos[0] = new(new(position.X - 1, position.Y), (byte)BlobDirection.West);
                    blobInfos[1] = new(new(position.X - 1, position.Y - 1), (byte)BlobDirection.Northwest);
                    blobInfos[2] = new(new(position.X, position.Y - 1), (byte)BlobDirection.North);
                    break;

                case 1:
                    blobInfos[0] = new(new(position.X + 1, position.Y), (byte)BlobDirection.East);
                    blobInfos[1] = new(new(position.X + 1, position.Y - 1), (byte)BlobDirection.Northeast);
                    blobInfos[2] = new(new(position.X, position.Y - 1), (byte)BlobDirection.North);
                    break;

                case 2:
                    blobInfos[0] = new(new(position.X - 1, position.Y), (byte)BlobDirection.West);
                    blobInfos[1] = new(new(position.X - 1, position.Y + 1), (byte)BlobDirection.Southwest);
                    blobInfos[2] = new(new(position.X, position.Y + 1), (byte)BlobDirection.South);
                    break;

                case 3:
                    blobInfos[0] = new(new(position.X + 1, position.Y), (byte)BlobDirection.East);
                    blobInfos[1] = new(new(position.X + 1, position.Y + 1), (byte)BlobDirection.Southeast);
                    blobInfos[2] = new(new(position.X, position.Y + 1), (byte)BlobDirection.South);
                    break;

                default:
                    blobInfos[0] = default;
                    blobInfos[1] = default;
                    blobInfos[2] = default;
                    break;
            }
        }

        private static void SetChunkSpriteFromIndexAndBlobValue(in int index, in byte blobValue)
        {
            switch (index)
            {
                case 0:
                    spriteClipAreas[index] = blobValue switch
                    {
                        ElementConstants.BLOB_NORTHWEST_PIVOT_EMPTY => spriteKeyPoints[(int)SpriteKeyPoints.Corner_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_1 => spriteKeyPoints[(int)SpriteKeyPoints.Horizontal_Edge_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_2 => spriteKeyPoints[(int)SpriteKeyPoints.Corner_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_3 => spriteKeyPoints[(int)SpriteKeyPoints.Vertical_Edge_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_4 => spriteKeyPoints[(int)SpriteKeyPoints.Horizontal_Edge_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_5 => spriteKeyPoints[(int)SpriteKeyPoints.Vertical_Edge_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_6 => spriteKeyPoints[(int)SpriteKeyPoints.Gap_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_SURROUNDED => spriteKeyPoints[(int)SpriteKeyPoints.Full_Northwest],
                        _ => spriteKeyPoints[(int)SpriteKeyPoints.Full_Northwest]
                    };
                    break;

                case 1:
                    spriteClipAreas[index] = blobValue switch
                    {
                        ElementConstants.BLOB_NORTHEAST_PIVOT_EMPTY => spriteKeyPoints[(int)SpriteKeyPoints.Corner_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_1 => spriteKeyPoints[(int)SpriteKeyPoints.Horizontal_Edge_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_2 => spriteKeyPoints[(int)SpriteKeyPoints.Corner_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_3 => spriteKeyPoints[(int)SpriteKeyPoints.Vertical_Edge_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_4 => spriteKeyPoints[(int)SpriteKeyPoints.Horizontal_Edge_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_5 => spriteKeyPoints[(int)SpriteKeyPoints.Vertical_Edge_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_6 => spriteKeyPoints[(int)SpriteKeyPoints.Gap_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_SURROUNDED => spriteKeyPoints[(int)SpriteKeyPoints.Full_Northeast],
                        _ => spriteKeyPoints[(int)SpriteKeyPoints.Full_Northeast]
                    };
                    break;

                case 2:
                    spriteClipAreas[index] = blobValue switch
                    {
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_EMPTY => spriteKeyPoints[(int)SpriteKeyPoints.Corner_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_1 => spriteKeyPoints[(int)SpriteKeyPoints.Horizontal_Edge_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_2 => spriteKeyPoints[(int)SpriteKeyPoints.Corner_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_3 => spriteKeyPoints[(int)SpriteKeyPoints.Vertical_Edge_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_4 => spriteKeyPoints[(int)SpriteKeyPoints.Horizontal_Edge_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_5 => spriteKeyPoints[(int)SpriteKeyPoints.Vertical_Edge_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_6 => spriteKeyPoints[(int)SpriteKeyPoints.Gap_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_SURROUNDED => spriteKeyPoints[(int)SpriteKeyPoints.Full_Southwest],
                        _ => spriteKeyPoints[(int)SpriteKeyPoints.Full_Southwest]
                    };
                    break;

                case 3:
                    spriteClipAreas[index] = blobValue switch
                    {
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_EMPTY => spriteKeyPoints[(int)SpriteKeyPoints.Corner_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_1 => spriteKeyPoints[(int)SpriteKeyPoints.Horizontal_Edge_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_2 => spriteKeyPoints[(int)SpriteKeyPoints.Corner_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_3 => spriteKeyPoints[(int)SpriteKeyPoints.Vertical_Edge_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_4 => spriteKeyPoints[(int)SpriteKeyPoints.Horizontal_Edge_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_5 => spriteKeyPoints[(int)SpriteKeyPoints.Vertical_Edge_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_6 => spriteKeyPoints[(int)SpriteKeyPoints.Gap_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_SURROUNDED => spriteKeyPoints[(int)SpriteKeyPoints.Full_Southeast],
                        _ => spriteKeyPoints[(int)SpriteKeyPoints.Full_Southeast]
                    };
                    break;

                default:
                    break;
            }
        }

        private static void CaptureBlobElementRoutine(ElementContext context, in ElementIndex elementIndex, SKCanvas canvas, in SKPointI textureOriginOffset)
        {
            SlotLayer slotLayer = context.Slot.GetLayer(context.Layer);
            Color colorModifier = TemperatureConstants.ApplyHeatColor(slotLayer.ColorModifier, slotLayer.Temperature);

            if (context.Layer == Layer.Background)
            {
                colorModifier = colorModifier.Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            Point slotPos = context.Slot.Position;
            UpdateSpritePositions(slotPos);

            using SKPaint paint = new()
            {
                Color = colorModifier.ToSKColor(),
                IsAntialias = false,
            };

            for (int i = 0; i < ElementConstants.SPRITE_DIVISIONS_LENGTH; i++)
            {
                UpdateSpriteSlice(context, elementIndex, i, slotPos);

                SKRect srcRect = new(
                    spriteClipAreas[i].Left + textureOriginOffset.X,
                    spriteClipAreas[i].Top + textureOriginOffset.Y,
                    spriteClipAreas[i].Right + textureOriginOffset.X,
                    spriteClipAreas[i].Bottom + textureOriginOffset.Y
                );

                SKRect destRect = new(
                    spritePositions[i].X,
                    spritePositions[i].Y,
                    spritePositions[i].X + ElementConstants.SPRITE_SLICE_SIZE,
                    spritePositions[i].Y + ElementConstants.SPRITE_SLICE_SIZE
                );

                canvas.DrawImage(elementsImage, srcRect, destRect, paint);
            }
        }

        private static void CaptureSingleElementRoutine(ElementContext context, SKCanvas canvas, in SKPointI textureOriginOffset)
        {
            SlotLayer slotLayer = context.Slot.GetLayer(context.Layer);
            Color colorModifier = TemperatureConstants.ApplyHeatColor(slotLayer.ColorModifier, slotLayer.Temperature);

            if (context.Layer == Layer.Background)
            {
                colorModifier = colorModifier.Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            float destX = (float)context.Slot.Position.X * WorldConstants.GRID_SIZE;
            float destY = (float)context.Slot.Position.Y * WorldConstants.GRID_SIZE;

            SKRect srcRect = new(
                textureOriginOffset.X,
                textureOriginOffset.Y,
                textureOriginOffset.X + 32,
                textureOriginOffset.Y + 32
            );

            SKRect destRect = new(
                destX,
                destY,
                destX + 32,
                destY + 32
            );

            using SKPaint paint = new()
            {
                Color = colorModifier.ToSKColor(),
                IsAntialias = false,
            };

            canvas.DrawImage(elementsImage, srcRect, destRect, paint);
        }

        internal static void Capture(ElementContext context, Element element, SKCanvas canvas, in SKPointI textureOriginOffset)
        {
            switch (element.RenderingType)
            {
                case ElementRenderingType.Single:
                    CaptureSingleElementRoutine(context, canvas, textureOriginOffset);
                    break;

                case ElementRenderingType.Blob:
                    CaptureBlobElementRoutine(context, element.Index, canvas, textureOriginOffset);
                    break;

                default:
                    break;
            }
        }
    }
}
