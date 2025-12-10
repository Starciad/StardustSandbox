using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements
{
    internal static class ElementRenderer
    {
        private readonly struct BlobInfo(Point position, byte blobValue)
        {
            public readonly Point Position => position;
            public readonly byte BlobValue => blobValue;
        }

        private static readonly BlobInfo[] blobInfos = new BlobInfo[3];
        private static readonly Vector2[] spritePositions = new Vector2[ElementConstants.SPRITE_DIVISIONS_LENGTH];
        private static readonly Rectangle[] spriteClipAreas = new Rectangle[ElementConstants.SPRITE_DIVISIONS_LENGTH];

        private static readonly Rectangle[] spriteKeyPoints = [
            // Full 0, 1, 2, 3
            new Rectangle(location: new Point(00, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(16, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(00, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(16, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),

            // Corner 4, 5, 6, 7
            new Rectangle(location: new Point(32, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(48, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(32, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(48, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),

            // Vertical Edge 8, 9, 10, 11
            new Rectangle(location: new Point(64, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(80, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(64, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(80, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),

            // Horizontal Border 12, 13, 14, 15
            new Rectangle(location: new Point(096, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(112, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(096, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(112, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),

            // Gaps 16, 17, 18, 19
            new Rectangle(location: new Point(128, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(144, 00), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(128, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(144, 16), size: new Point(ElementConstants.SPRITE_SLICE_SIZE)),
        ];

        #region BLOB RENDERING

        private static void UpdateSpritePositions(Point position)
        {
            float xOffset = ElementConstants.SPRITE_X_OFFSET, yOffset = ElementConstants.SPRITE_Y_OFFSET;

            spritePositions[0] = new Vector2(position.X, position.Y) * WorldConstants.GRID_SIZE;
            spritePositions[1] = new Vector2(position.X + xOffset, position.Y) * WorldConstants.GRID_SIZE;
            spritePositions[2] = new Vector2(position.X, position.Y + yOffset) * WorldConstants.GRID_SIZE;
            spritePositions[3] = new Vector2(position.X + xOffset, position.Y + yOffset) * WorldConstants.GRID_SIZE;
        }

        private static void UpdateSpriteSlice(in ElementContext context, Element element, int index, Point position)
        {
            SetChunkSpriteFromIndexAndBlobValue(index, GetBlobValueFromTargetPositions(context, element, index, position));
        }

        private static byte GetBlobValueFromTargetPositions(in ElementContext context, Element element, int index, Point position)
        {
            byte result = 0;

            GetTargetPositionsFromIndex(index, position);

            // Check each of the target positions.
            for (int i = 0; i < blobInfos.Length; i++)
            {
                // Get element from target position.
                if (context.TryGetElement(blobInfos[i].Position, context.Layer, out Element value))
                {
                    // Check conditions for addition to blob value. If you fail, just continue to the next iteration.
                    if (value != element)
                    {
                        continue;
                    }

                    // Upon successful completion of the conditions and steps, add to the blob value.
                    result += blobInfos[i].BlobValue;
                }
            }

            return result;
        }

        private static void GetTargetPositionsFromIndex(int index, Point position)
        {
            switch (index)
            {
                // Sprite Piece 1 (Northwest Pivot)
                case 0:
                    blobInfos[0] = new(new Point(position.X - 1, position.Y), (byte)BlobCardinalDirection.West);
                    blobInfos[1] = new(new Point(position.X - 1, position.Y - 1), (byte)BlobCardinalDirection.Northwest);
                    blobInfos[2] = new(new Point(position.X, position.Y - 1), (byte)BlobCardinalDirection.North);
                    break;

                // Sprite Piece 2 (Northeast Pivot)
                case 1:
                    blobInfos[0] = new(new Point(position.X + 1, position.Y), (byte)BlobCardinalDirection.East);
                    blobInfos[1] = new(new Point(position.X + 1, position.Y - 1), (byte)BlobCardinalDirection.Northeast);
                    blobInfos[2] = new(new Point(position.X, position.Y - 1), (byte)BlobCardinalDirection.North);
                    break;

                // Sprite Piece 3 (Southwest Pivot)
                case 2:
                    blobInfos[0] = new(new Point(position.X - 1, position.Y), (byte)BlobCardinalDirection.West);
                    blobInfos[1] = new(new Point(position.X - 1, position.Y + 1), (byte)BlobCardinalDirection.Southwest);
                    blobInfos[2] = new(new Point(position.X, position.Y + 1), (byte)BlobCardinalDirection.South);
                    break;

                // Sprite Piece 4 (Southeast Pivot)
                case 3:
                    blobInfos[0] = new(new Point(position.X + 1, position.Y), (byte)BlobCardinalDirection.East);
                    blobInfos[1] = new(new Point(position.X + 1, position.Y + 1), (byte)BlobCardinalDirection.Southeast);
                    blobInfos[2] = new(new Point(position.X, position.Y + 1), (byte)BlobCardinalDirection.South);
                    break;

                default:
                    blobInfos[0] = default;
                    blobInfos[1] = default;
                    blobInfos[2] = default;
                    break;
            }
        }

        private static void SetChunkSpriteFromIndexAndBlobValue(int index, byte blobValue)
        {
            switch (index)
            {
                // (Sprite 1 - Northwest Pivot)
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
                        _ => spriteKeyPoints[(int)SpriteKeyPoints.Full_Northwest],
                    };
                    break;

                // (Sprite 2 - Northeast Pivot)
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
                        _ => spriteKeyPoints[(int)SpriteKeyPoints.Full_Northeast],
                    };
                    break;

                // (Sprite 3 - Southwest Pivot)
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
                        _ => spriteKeyPoints[(int)SpriteKeyPoints.Full_Southwest],
                    };
                    break;

                // (Sprite 4 - Southeast Pivot)
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
                        _ => spriteKeyPoints[(int)SpriteKeyPoints.Full_Southeast],
                    };
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region DRAWING LOGIC

        private static void DrawBlobElementRoutine(in ElementContext context, Element element, SpriteBatch spriteBatch, Point textureOriginOffset)
        {
            SlotLayer slotLayer = context.Slot.GetLayer(context.Layer);

            Color colorModifier = TemperatureConstants.ApplyHeatColor(slotLayer.ColorModifier, slotLayer.Temperature);

            if (context.Layer == Layer.Background)
            {
                colorModifier = colorModifier.Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            UpdateSpritePositions(context.Slot.Position);

            for (int i = 0; i < ElementConstants.SPRITE_DIVISIONS_LENGTH; i++)
            {
                UpdateSpriteSlice(context, element, i, context.Slot.Position);
                spriteBatch.Draw(
                    AssetDatabase.GetTexture(TextureIndex.Elements),
                    spritePositions[i],
                    new(textureOriginOffset + spriteClipAreas[i].Location, spriteClipAreas[i].Size),
                    colorModifier,
                    0.0f,
                    Vector2.Zero,
                    Vector2.One,
                    SpriteEffects.None,
                    0.0f
                );
            }
        }

        private static void DrawSingleElementRoutine(in ElementContext context, SpriteBatch spriteBatch, Point textureOriginOffset)
        {
            SlotLayer slotLayer = context.Slot.GetLayer(context.Layer);

            Color colorModifier = TemperatureConstants.ApplyHeatColor(slotLayer.ColorModifier, slotLayer.Temperature);

            if (context.Layer == Layer.Background)
            {
                colorModifier = colorModifier.Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            spriteBatch.Draw(
                AssetDatabase.GetTexture(TextureIndex.Elements),
                new Vector2(context.Slot.Position.X, context.Slot.Position.Y) * WorldConstants.GRID_SIZE,
                new Rectangle(textureOriginOffset, new(32)),
                colorModifier,
                0f,
                Vector2.Zero,
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }

        internal static void Draw(in ElementContext context, Element element, SpriteBatch spriteBatch, Point textureOriginOffset)
        {
            // Handle blob tiles separately.
            switch (element.RenderingType)
            {
                case ElementRenderingType.Single:
                    DrawSingleElementRoutine(context, spriteBatch, textureOriginOffset);
                    break;

                case ElementRenderingType.Blob:
                    DrawBlobElementRoutine(context, element, spriteBatch, textureOriginOffset);
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
