using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Constants.Elements;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Enums.Elements;
using PixelDust.Game.Enums.General;

namespace PixelDust.Game.Elements.Rendering.Common
{
    public sealed partial class PElementBlobRenderingMechanism : PElementRenderingMechanism
    {
        private static readonly Color color = Color.White;
        private static readonly float rotation = 0f;
        private static readonly Vector2 origin = Vector2.Zero;
        private static readonly Vector2 scale = Vector2.One;
        private static readonly SpriteEffects spriteEffects = SpriteEffects.None;
        private static readonly float layerDepth = 0f;

        private readonly Vector2[] spritePositions = new Vector2[PElementRenderingConstants.SPRITE_DIVISIONS_LENGTH];
        private readonly Rectangle[] spriteClipAreas = new Rectangle[PElementRenderingConstants.SPRITE_DIVISIONS_LENGTH];

        private PElement element;
        private Texture2D elementTexture;

        // Overrides
        protected override void OnInitialize(PElement element)
        {
            this.element = element;
            this.elementTexture = element.Texture;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch, PElementContext context)
        {
            Point position = context.Position;

            UpdateSpritePositions(position);

            for (int i = 0; i < PElementRenderingConstants.SPRITE_DIVISIONS_LENGTH; i++)
            {
                UpdateSpriteSlice(context, i, position);
                spriteBatch.Draw(this.elementTexture, this.spritePositions[i], this.spriteClipAreas[i], color, rotation, origin, scale, spriteEffects, layerDepth);
            }
        }

        // Updates
        private void UpdateSpritePositions(Point position)
        {
            float xOffset = PElementRenderingConstants.SPRITE_X_OFFSET, yOffset = PElementRenderingConstants.SPRITE_Y_OFFSET;

            this.spritePositions[0] = new Vector2(position.X, position.Y) * PWorldConstants.GRID_SCALE;
            this.spritePositions[1] = new Vector2(position.X + xOffset, position.Y) * PWorldConstants.GRID_SCALE;
            this.spritePositions[2] = new Vector2(position.X, position.Y + yOffset) * PWorldConstants.GRID_SCALE;
            this.spritePositions[3] = new Vector2(position.X + xOffset, position.Y + yOffset) * PWorldConstants.GRID_SCALE;
        }

        private void UpdateSpriteSlice(PElementContext context, int index, Point position)
        {
            SetChunkSpriteFromIndexAndBlobValue(index, GetBlobValueFromTargetPositions(context, GetTargetPositionsFromIndex(index, position)));
        }

        private static (byte, Point)[] GetTargetPositionsFromIndex(int index, Point position)
        {
            return index switch
            {
                // Sprite Piece 1 (Northwest Pivot)
                0 => [
                    ((byte)PBlobCardinalDirection.West, new Point(position.X - 1, position.Y)),
                    ((byte)PBlobCardinalDirection.Northwest, new Point(position.X - 1, position.Y - 1)),
                    ((byte)PBlobCardinalDirection.North, new Point(position.X, position.Y - 1))
                ],

                // Sprite Piece 2 (Northeast Pivot)
                1 => [
                    ((byte)PBlobCardinalDirection.East, new Point(position.X + 1, position.Y)),
                    ((byte)PBlobCardinalDirection.Northeast, new Point(position.X + 1, position.Y - 1)),
                    ((byte)PBlobCardinalDirection.North, new Point(position.X, position.Y - 1))
                ],

                // Sprite Piece 3 (Southwest Pivot)
                2 => [
                    ((byte)PBlobCardinalDirection.West, new Point(position.X - 1, position.Y)),
                    ((byte)PBlobCardinalDirection.Southwest, new Point(position.X - 1, position.Y + 1)),
                    ((byte)PBlobCardinalDirection.South, new Point(position.X, position.Y + 1))
                ],

                // Sprite Piece 4 (Southeast Pivot)
                3 => [
                    ((byte)PBlobCardinalDirection.East, new Point(position.X + 1, position.Y)),
                    ((byte)PBlobCardinalDirection.Southeast, new Point(position.X + 1, position.Y + 1)),
                    ((byte)PBlobCardinalDirection.South, new Point(position.X, position.Y + 1))
                ],

                _ => [],
            };
        }

        private byte GetBlobValueFromTargetPositions(PElementContext context, (byte blobValue, Point position)[] targets)
        {
            byte result = 0;

            // Check each of the target positions.
            for (int i = 0; i < targets.Length; i++)
            {
                // Get element from target position.
                if (context.TryGetElement(targets[i].position, out PElement value))
                {
                    // Check conditions for addition to blob value. If you fail, just continue to the next iteration.
                    if (value != this.element)
                    {
                        continue;
                    }

                    // Upon successful completion of the conditions and steps, add to the blob value.
                    result += targets[i].blobValue;
                }
            }

            return result;
        }

        private void SetChunkSpriteFromIndexAndBlobValue(int index, byte blobValue)
        {
            switch (index)
            {
                // (Sprite 1 - Northwest Pivot)
                case 0:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_EMPTY => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_1 => spriteKeyPoints[(int)PSpriteKeyPoints.Horizontal_Edge_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_2 => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_3 => spriteKeyPoints[(int)PSpriteKeyPoints.Vertical_Edge_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_4 => spriteKeyPoints[(int)PSpriteKeyPoints.Horizontal_Edge_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_5 => spriteKeyPoints[(int)PSpriteKeyPoints.Vertical_Edge_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_6 => spriteKeyPoints[(int)PSpriteKeyPoints.Gap_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_SURROUNDED => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        _ => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                    };
                    break;

                // (Sprite 2 - Northeast Pivot)
                case 1:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_EMPTY => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Northeast],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_1 => spriteKeyPoints[(int)PSpriteKeyPoints.Horizontal_Edge_Northeast],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_2 => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Northeast],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_3 => spriteKeyPoints[(int)PSpriteKeyPoints.Vertical_Edge_Northeast],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_4 => spriteKeyPoints[(int)PSpriteKeyPoints.Horizontal_Edge_Northeast],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_5 => spriteKeyPoints[(int)PSpriteKeyPoints.Vertical_Edge_Northeast],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_6 => spriteKeyPoints[(int)PSpriteKeyPoints.Gap_Northeast],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_SURROUNDED => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northeast],
                        _ => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northeast],
                    };
                    break;

                // (Sprite 3 - Southwest Pivot)
                case 2:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_EMPTY => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Southwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_1 => spriteKeyPoints[(int)PSpriteKeyPoints.Horizontal_Edge_Southwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_2 => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Southwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_3 => spriteKeyPoints[(int)PSpriteKeyPoints.Vertical_Edge_Southwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_4 => spriteKeyPoints[(int)PSpriteKeyPoints.Horizontal_Edge_Southwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_5 => spriteKeyPoints[(int)PSpriteKeyPoints.Vertical_Edge_Southwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_6 => spriteKeyPoints[(int)PSpriteKeyPoints.Gap_Southwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_SURROUNDED => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Southwest],
                        _ => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Southwest],
                    };
                    break;

                // (Sprite 4 - Southeast Pivot)
                case 3:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_EMPTY => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Southeast],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_1 => spriteKeyPoints[(int)PSpriteKeyPoints.Horizontal_Edge_Southeast],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_2 => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Southeast],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_3 => spriteKeyPoints[(int)PSpriteKeyPoints.Vertical_Edge_Southeast],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_4 => spriteKeyPoints[(int)PSpriteKeyPoints.Horizontal_Edge_Southeast],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_5 => spriteKeyPoints[(int)PSpriteKeyPoints.Vertical_Edge_Southeast],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_6 => spriteKeyPoints[(int)PSpriteKeyPoints.Gap_Southeast],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_SURROUNDED => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Southeast],
                        _ => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Southeast],
                    };
                    break;

                default:
                    break;
            }
        }
    }
}
