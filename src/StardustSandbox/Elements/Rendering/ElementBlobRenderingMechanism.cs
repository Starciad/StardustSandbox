using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Extensions;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Elements.Rendering
{
    internal sealed class ElementBlobRenderingMechanism : ElementRenderingMechanism
    {
        private readonly struct BlobInfo(Point position, byte blobValue)
        {
            internal readonly Point Position => position;
            internal readonly byte BlobValue => blobValue;
        }

        private Element element;
        private Texture2D elementTexture;

        private readonly BlobInfo[] blobInfos = new BlobInfo[3];
        private readonly Vector2[] spritePositions = new Vector2[ElementConstants.SPRITE_DIVISIONS_LENGTH];
        private readonly Rectangle[] spriteClipAreas = new Rectangle[ElementConstants.SPRITE_DIVISIONS_LENGTH];

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

        internal override void Initialize(Element element)
        {
            this.element = element;
            this.elementTexture = element.Texture;
        }

        internal override void Draw(SpriteBatch spriteBatch, ElementContext context)
        {
            SlotLayer worldSlotLayer = context.Slot.GetLayer(context.Layer);

            Color colorModifier = TemperatureConstants.ApplyHeatColor(worldSlotLayer.ColorModifier, worldSlotLayer.Temperature);

            if (context.Layer == LayerType.Background)
            {
                colorModifier = colorModifier.Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            UpdateSpritePositions(context.Slot.Position);

            for (byte i = 0; i < ElementConstants.SPRITE_DIVISIONS_LENGTH; i++)
            {
                UpdateSpriteSlice(context, i, context.Slot.Position);
                spriteBatch.Draw(this.elementTexture, this.spritePositions[i], this.spriteClipAreas[i], colorModifier, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
            }
        }

        // Updates
        private void UpdateSpritePositions(Point position)
        {
            float xOffset = ElementConstants.SPRITE_X_OFFSET, yOffset = ElementConstants.SPRITE_Y_OFFSET;

            this.spritePositions[0] = new Vector2(position.X, position.Y) * WorldConstants.GRID_SIZE;
            this.spritePositions[1] = new Vector2(position.X + xOffset, position.Y) * WorldConstants.GRID_SIZE;
            this.spritePositions[2] = new Vector2(position.X, position.Y + yOffset) * WorldConstants.GRID_SIZE;
            this.spritePositions[3] = new Vector2(position.X + xOffset, position.Y + yOffset) * WorldConstants.GRID_SIZE;
        }

        private void UpdateSpriteSlice(ElementContext context, byte index, Point position)
        {
            SetChunkSpriteFromIndexAndBlobValue(index, GetBlobValueFromTargetPositions(context, index, position));
        }

        private byte GetBlobValueFromTargetPositions(ElementContext context, byte index, Point position)
        {
            byte result = 0;

            GetTargetPositionsFromIndex(index, position);

            // Check each of the target positions.
            for (byte i = 0; i < this.blobInfos.Length; i++)
            {
                // Get element from target position.
                if (context.TryGetElement(this.blobInfos[i].Position, context.Layer, out Element value))
                {
                    // Check conditions for addition to blob value. If you fail, just continue to the next iteration.
                    if (value != this.element)
                    {
                        continue;
                    }

                    // Upon successful completion of the conditions and steps, add to the blob value.
                    result += this.blobInfos[i].BlobValue;
                }
            }

            return result;
        }

        private void GetTargetPositionsFromIndex(byte index, Point position)
        {
            switch (index)
            {
                // Sprite Piece 1 (Northwest Pivot)
                case 0:
                    this.blobInfos[0] = new(new Point(position.X - 1, position.Y), (byte)BlobCardinalDirection.West);
                    this.blobInfos[1] = new(new Point(position.X - 1, position.Y - 1), (byte)BlobCardinalDirection.Northwest);
                    this.blobInfos[2] = new(new Point(position.X, position.Y - 1), (byte)BlobCardinalDirection.North);
                    break;

                // Sprite Piece 2 (Northeast Pivot)
                case 1:
                    this.blobInfos[0] = new(new Point(position.X + 1, position.Y), (byte)BlobCardinalDirection.East);
                    this.blobInfos[1] = new(new Point(position.X + 1, position.Y - 1), (byte)BlobCardinalDirection.Northeast);
                    this.blobInfos[2] = new(new Point(position.X, position.Y - 1), (byte)BlobCardinalDirection.North);
                    break;

                // Sprite Piece 3 (Southwest Pivot)
                case 2:
                    this.blobInfos[0] = new(new Point(position.X - 1, position.Y), (byte)BlobCardinalDirection.West);
                    this.blobInfos[1] = new(new Point(position.X - 1, position.Y + 1), (byte)BlobCardinalDirection.Southwest);
                    this.blobInfos[2] = new(new Point(position.X, position.Y + 1), (byte)BlobCardinalDirection.South);
                    break;

                // Sprite Piece 4 (Southeast Pivot)
                case 3:
                    this.blobInfos[0] = new(new Point(position.X + 1, position.Y), (byte)BlobCardinalDirection.East);
                    this.blobInfos[1] = new(new Point(position.X + 1, position.Y + 1), (byte)BlobCardinalDirection.Southeast);
                    this.blobInfos[2] = new(new Point(position.X, position.Y + 1), (byte)BlobCardinalDirection.South);
                    break;

                default:
                    this.blobInfos[0] = default;
                    this.blobInfos[1] = default;
                    this.blobInfos[2] = default;
                    break;
            }
        }

        private void SetChunkSpriteFromIndexAndBlobValue(int index, byte blobValue)
        {
            switch (index)
            {
                // (Sprite 1 - Northwest Pivot)
                case 0:
                    this.spriteClipAreas[index] = blobValue switch
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
                    this.spriteClipAreas[index] = blobValue switch
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
                    this.spriteClipAreas[index] = blobValue switch
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
                    this.spriteClipAreas[index] = blobValue switch
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
    }
}
