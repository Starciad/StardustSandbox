using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Constants.Elements;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Elements.Contexts;
using StardustSandbox.Game.Elements.Rendering;
using StardustSandbox.Game.Enums.Elements;
using StardustSandbox.Game.Enums.General;

namespace StardustSandbox.Game.GameContent.Elements.Rendering
{
    public sealed partial class SElementBlobRenderingMechanism : SElementRenderingMechanism
    {
        private static readonly Color color = Color.White;
        private static readonly float rotation = 0f;
        private static readonly Vector2 origin = Vector2.Zero;
        private static readonly Vector2 scale = Vector2.One;
        private static readonly SpriteEffects spriteEffects = SpriteEffects.None;
        private static readonly float layerDepth = 0f;

        private readonly Vector2[] spritePositions = new Vector2[SElementRenderingConstants.SPRITE_DIVISIONS_LENGTH];
        private readonly Rectangle[] spriteClipAreas = new Rectangle[SElementRenderingConstants.SPRITE_DIVISIONS_LENGTH];

        private SElement element;
        private Texture2D elementTexture;

        public override void Initialize(SElement element)
        {
            this.element = element;
            this.elementTexture = element.Texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, ISElementContext context)
        {
            Point position = context.Position;

            UpdateSpritePositions(position);

            for (int i = 0; i < SElementRenderingConstants.SPRITE_DIVISIONS_LENGTH; i++)
            {
                UpdateSpriteSlice(context, i, position);
                spriteBatch.Draw(this.elementTexture, this.spritePositions[i], this.spriteClipAreas[i], color, rotation, origin, scale, spriteEffects, layerDepth);
            }
        }

        // Updates
        private void UpdateSpritePositions(Point position)
        {
            float xOffset = SElementRenderingConstants.SPRITE_X_OFFSET, yOffset = SElementRenderingConstants.SPRITE_Y_OFFSET;

            this.spritePositions[0] = new Vector2(position.X, position.Y) * SWorldConstants.GRID_SCALE;
            this.spritePositions[1] = new Vector2(position.X + xOffset, position.Y) * SWorldConstants.GRID_SCALE;
            this.spritePositions[2] = new Vector2(position.X, position.Y + yOffset) * SWorldConstants.GRID_SCALE;
            this.spritePositions[3] = new Vector2(position.X + xOffset, position.Y + yOffset) * SWorldConstants.GRID_SCALE;
        }

        private void UpdateSpriteSlice(ISElementContext context, int index, Point position)
        {
            SetChunkSpriteFromIndexAndBlobValue(index, GetBlobValueFromTargetPositions(context, GetTargetPositionsFromIndex(index, position)));
        }

        private static (byte, Point)[] GetTargetPositionsFromIndex(int index, Point position)
        {
            return index switch
            {
                // Sprite Piece 1 (Northwest Pivot)
                0 => [
                    ((byte)SBlobCardinalDirection.West, new Point(position.X - 1, position.Y)),
                    ((byte)SBlobCardinalDirection.Northwest, new Point(position.X - 1, position.Y - 1)),
                    ((byte)SBlobCardinalDirection.North, new Point(position.X, position.Y - 1))
                ],

                // Sprite Piece 2 (Northeast Pivot)
                1 => [
                    ((byte)SBlobCardinalDirection.East, new Point(position.X + 1, position.Y)),
                    ((byte)SBlobCardinalDirection.Northeast, new Point(position.X + 1, position.Y - 1)),
                    ((byte)SBlobCardinalDirection.North, new Point(position.X, position.Y - 1))
                ],

                // Sprite Piece 3 (Southwest Pivot)
                2 => [
                    ((byte)SBlobCardinalDirection.West, new Point(position.X - 1, position.Y)),
                    ((byte)SBlobCardinalDirection.Southwest, new Point(position.X - 1, position.Y + 1)),
                    ((byte)SBlobCardinalDirection.South, new Point(position.X, position.Y + 1))
                ],

                // Sprite Piece 4 (Southeast Pivot)
                3 => [
                    ((byte)SBlobCardinalDirection.East, new Point(position.X + 1, position.Y)),
                    ((byte)SBlobCardinalDirection.Southeast, new Point(position.X + 1, position.Y + 1)),
                    ((byte)SBlobCardinalDirection.South, new Point(position.X, position.Y + 1))
                ],

                _ => [],
            };
        }

        private byte GetBlobValueFromTargetPositions(ISElementContext context, (byte blobValue, Point position)[] targets)
        {
            byte result = 0;

            // Check each of the target positions.
            for (int i = 0; i < targets.Length; i++)
            {
                // Get element from target position.
                if (context.TryGetElement(targets[i].position, out SElement value))
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
                        SElementRenderingConstants.BLOB_NORTHWEST_PIVOT_EMPTY => spriteKeyPoints[(int)SSpriteKeyPoints.Corner_Northwest],
                        SElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_1 => spriteKeyPoints[(int)SSpriteKeyPoints.Horizontal_Edge_Northwest],
                        SElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_2 => spriteKeyPoints[(int)SSpriteKeyPoints.Corner_Northwest],
                        SElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_3 => spriteKeyPoints[(int)SSpriteKeyPoints.Vertical_Edge_Northwest],
                        SElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_4 => spriteKeyPoints[(int)SSpriteKeyPoints.Horizontal_Edge_Northwest],
                        SElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_5 => spriteKeyPoints[(int)SSpriteKeyPoints.Vertical_Edge_Northwest],
                        SElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_6 => spriteKeyPoints[(int)SSpriteKeyPoints.Gap_Northwest],
                        SElementRenderingConstants.BLOB_NORTHWEST_PIVOT_SURROUNDED => spriteKeyPoints[(int)SSpriteKeyPoints.Full_Northwest],
                        _ => spriteKeyPoints[(int)SSpriteKeyPoints.Full_Northwest],
                    };
                    break;

                // (Sprite 2 - Northeast Pivot)
                case 1:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        SElementRenderingConstants.BLOB_NORTHEAST_PIVOT_EMPTY => spriteKeyPoints[(int)SSpriteKeyPoints.Corner_Northeast],
                        SElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_1 => spriteKeyPoints[(int)SSpriteKeyPoints.Horizontal_Edge_Northeast],
                        SElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_2 => spriteKeyPoints[(int)SSpriteKeyPoints.Corner_Northeast],
                        SElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_3 => spriteKeyPoints[(int)SSpriteKeyPoints.Vertical_Edge_Northeast],
                        SElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_4 => spriteKeyPoints[(int)SSpriteKeyPoints.Horizontal_Edge_Northeast],
                        SElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_5 => spriteKeyPoints[(int)SSpriteKeyPoints.Vertical_Edge_Northeast],
                        SElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_6 => spriteKeyPoints[(int)SSpriteKeyPoints.Gap_Northeast],
                        SElementRenderingConstants.BLOB_NORTHEAST_PIVOT_SURROUNDED => spriteKeyPoints[(int)SSpriteKeyPoints.Full_Northeast],
                        _ => spriteKeyPoints[(int)SSpriteKeyPoints.Full_Northeast],
                    };
                    break;

                // (Sprite 3 - Southwest Pivot)
                case 2:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        SElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_EMPTY => spriteKeyPoints[(int)SSpriteKeyPoints.Corner_Southwest],
                        SElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_1 => spriteKeyPoints[(int)SSpriteKeyPoints.Horizontal_Edge_Southwest],
                        SElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_2 => spriteKeyPoints[(int)SSpriteKeyPoints.Corner_Southwest],
                        SElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_3 => spriteKeyPoints[(int)SSpriteKeyPoints.Vertical_Edge_Southwest],
                        SElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_4 => spriteKeyPoints[(int)SSpriteKeyPoints.Horizontal_Edge_Southwest],
                        SElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_5 => spriteKeyPoints[(int)SSpriteKeyPoints.Vertical_Edge_Southwest],
                        SElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_6 => spriteKeyPoints[(int)SSpriteKeyPoints.Gap_Southwest],
                        SElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_SURROUNDED => spriteKeyPoints[(int)SSpriteKeyPoints.Full_Southwest],
                        _ => spriteKeyPoints[(int)SSpriteKeyPoints.Full_Southwest],
                    };
                    break;

                // (Sprite 4 - Southeast Pivot)
                case 3:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        SElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_EMPTY => spriteKeyPoints[(int)SSpriteKeyPoints.Corner_Southeast],
                        SElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_1 => spriteKeyPoints[(int)SSpriteKeyPoints.Horizontal_Edge_Southeast],
                        SElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_2 => spriteKeyPoints[(int)SSpriteKeyPoints.Corner_Southeast],
                        SElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_3 => spriteKeyPoints[(int)SSpriteKeyPoints.Vertical_Edge_Southeast],
                        SElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_4 => spriteKeyPoints[(int)SSpriteKeyPoints.Horizontal_Edge_Southeast],
                        SElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_5 => spriteKeyPoints[(int)SSpriteKeyPoints.Vertical_Edge_Southeast],
                        SElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_6 => spriteKeyPoints[(int)SSpriteKeyPoints.Gap_Southeast],
                        SElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_SURROUNDED => spriteKeyPoints[(int)SSpriteKeyPoints.Full_Southeast],
                        _ => spriteKeyPoints[(int)SSpriteKeyPoints.Full_Southeast],
                    };
                    break;

                default:
                    break;
            }
        }
    }
}
