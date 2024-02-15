using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants.Elements;
using PixelDust.Game.Elements.Contexts;
using PixelDust.Game.Enums.Elements;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Mathematics;
using PixelDust.Game.World.Slots;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace PixelDust.Game.Elements.Rendering.Common
{
    public sealed class PElementDynamicRendering : PElementRenderingMechanism
    {
        private static readonly Rectangle[] spriteKeyPoints = [
            // Full
            new Rectangle(location: new Point(00, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(16, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(00, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(16, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Corner
            new Rectangle(location: new Point(32, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(48, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(32, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(48, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Vertical Edge
            new Rectangle(location: new Point(64, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(80, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(64, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(80, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Horizontal Border
            new Rectangle(location: new Point(096, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(112, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(096, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(112, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),

            // Gaps
            new Rectangle(location: new Point(128, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(144, 00), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(128, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
            new Rectangle(location: new Point(144, 16), size: new Point(PElementRenderingConstants.SPRITE_SLICE_SIZE)),
        ];

        private static readonly Color color = Color.White;
        private static readonly float rotation = 0f;
        private static readonly Vector2 origin = Vector2.Zero;
        private static readonly Vector2 scale = Vector2.One;
        private static readonly SpriteEffects spriteEffects = SpriteEffects.None;
        private static readonly float layerDepth = 0f;

        private readonly Vector2[] spritePositions = new Vector2[PElementRenderingConstants.SPRITE_DIVISIONS_LENGTH];
        private readonly Rectangle[] spriteClipAreas = new Rectangle[PElementRenderingConstants.SPRITE_DIVISIONS_LENGTH];

        // Overrides
        public override void Initialize(PElement element)
        {
            base.Initialize(element);
        }

        public override void Update(GameTime gameTime, PElementContext context)
        {
            for (int i = 0; i < PElementRenderingConstants.SPRITE_DIVISIONS_LENGTH; i++)
            {
                UpdateSpriteSlice(context, i, context.Position);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, PElementContext context)
        {
            Texture2D texture = this.Element.Texture;

            for (int i = 0; i < PElementRenderingConstants.SPRITE_DIVISIONS_LENGTH; i++)
            {
                spriteBatch.Draw(texture, this.spritePositions[i], this.spriteClipAreas[i], color, rotation, origin, scale, spriteEffects, layerDepth);
            }
        }

        // Updates
        private void UpdateSpriteSlice(PElementContext context, int index, Vector2Int position)
        {
            // Get blob connection value.
            int blobValue = GetBlobValueFromTargetPositions(context, GetTargetPositionsFromIndex(index, position));

            // Rotate blob value based on current index.
            blobValue *= PElementRenderingConstants.BLOB_ROTATION_VALUE * (index + 1);
            if (blobValue > byte.MaxValue)
            {
                blobValue -= byte.MaxValue;
            }

            // Define the sprite to be used.
            SetChunkSpriteFromIndexAndBlobValue(index, (byte)blobValue);

        }

        private static (byte, Vector2Int)[] GetTargetPositionsFromIndex(int index, Vector2Int position)
        {
            return index switch
            {
                // Sprite Piece 1 (Northwest Pivot)
                0 => [
                    ((byte)PBlobCardinalDirection.West, new Vector2Int(position.X - 1, position.Y)),
                    ((byte)PBlobCardinalDirection.Northwest, new Vector2Int(position.X - 1, position.Y - 1)),
                    ((byte)PBlobCardinalDirection.North, new Vector2Int(position.X, position.Y - 1))
                ],

                // Sprite Piece 2 (Northeast Pivot)
                1 => [
                    ((byte)PBlobCardinalDirection.West, new Vector2Int(position.X + 1, position.Y)),
                    ((byte)PBlobCardinalDirection.Northeast, new Vector2Int(position.X + 1, position.Y - 1)),
                    ((byte)PBlobCardinalDirection.North, new Vector2Int(position.X, position.Y - 1))
                ],

                // Sprite Piece 3 (Southwest Pivot)
                2 => [
                    ((byte)PBlobCardinalDirection.West, new Vector2Int(position.X - 1, position.Y)),
                    ((byte)PBlobCardinalDirection.Southwest, new Vector2Int(position.X - 1, position.Y + 1)),
                    ((byte)PBlobCardinalDirection.South, new Vector2Int(position.X, position.Y + 1))
                ],

                // Sprite Piece 4 (Southeast Pivot)
                3 => [
                    ((byte)PBlobCardinalDirection.East, new Vector2Int(position.X + 1, position.Y)),
                    ((byte)PBlobCardinalDirection.Southeast, new Vector2Int(position.X + 1, position.Y + 1)),
                    ((byte)PBlobCardinalDirection.South, new Vector2Int(position.X, position.Y + 1))
                ],

                _ => [],
            };
        }

        private static byte GetBlobValueFromTargetPositions(PElementContext context, (byte blobValue, Vector2Int position)[] targets)
        {
            byte result = 0;

            for (int i = 0; i < targets.Length; i++)
            {
                if (context.TryGetElement(targets[i].position, out PElement _))
                {
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
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_1 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_2 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_3 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_4 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_5 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_CASE_6 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHWEST_PIVOT_SURROUNDED => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        _ => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                    };
                    break;

                // (Sprite 2 - Northeast Pivot)
                case 1:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_EMPTY => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Northwest],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_1 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_2 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_3 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_4 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_5 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_CASE_6 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_NORTHEAST_PIVOT_SURROUNDED => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        _ => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                    };
                    break;

                // (Sprite 3 - Southeast Pivot)
                case 2:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_EMPTY => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_1 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_2 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_3 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_4 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_5 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_CASE_6 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHEAST_PIVOT_SURROUNDED => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        _ => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                    };
                    break;

                // (Sprite 4 - Southwest Pivot)
                case 3:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_EMPTY => spriteKeyPoints[(int)PSpriteKeyPoints.Corner_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_1 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_2 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_3 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_4 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_5 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_CASE_6 => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        PElementRenderingConstants.BLOB_SOUTHWEST_PIVOT_SURROUNDED => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                        _ => spriteKeyPoints[(int)PSpriteKeyPoints.Full_Northwest],
                    };
                    break;

                default:
                    break;
            }
        }
    }
}
