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

using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.WorldSystem.Handlers.Rendering
{
    internal sealed class BlobElementRenderer : ElementRenderer
    {
        private readonly struct BlobInfo(Point position, byte blobValue)
        {
            internal readonly Point Position => position;
            internal readonly byte BlobValue => blobValue;
        }

        private readonly BlobInfo[] blobInfos = new BlobInfo[3];
        private readonly Vector2[] spritePositions = new Vector2[ElementConstants.SPRITE_DIVISIONS_LENGTH];
        private readonly Rectangle[] spriteClipAreas = new Rectangle[ElementConstants.SPRITE_DIVISIONS_LENGTH];

        internal BlobElementRenderer(AssetDatabase assetDatabase, Camera2D camera, GameplaySettings gameplaySettings) : base(assetDatabase, camera, gameplaySettings)
        {

        }

        private void UpdateSpritePositions(Point position)
        {
            float xOffset = ElementConstants.SPRITE_X_OFFSET, yOffset = ElementConstants.SPRITE_Y_OFFSET;

            this.spritePositions[0] = new Vector2(position.X, position.Y) * WorldConstants.TILE_SIZE;
            this.spritePositions[1] = new Vector2(position.X + xOffset, position.Y) * WorldConstants.TILE_SIZE;
            this.spritePositions[2] = new Vector2(position.X, position.Y + yOffset) * WorldConstants.TILE_SIZE;
            this.spritePositions[3] = new Vector2(position.X + xOffset, position.Y + yOffset) * WorldConstants.TILE_SIZE;
        }

        private void UpdateSpriteSlice(ElementContext context, int index, Point position)
        {
            SetChunkSpriteFromIndexAndBlobValue(index, GetBlobValueFromTargetPositions(context, index, position));
        }

        private byte GetBlobValueFromTargetPositions(ElementContext context, int index, Point position)
        {
            byte result = 0;

            GetTargetPositionsFromIndex(index, position);

            // Check each of the target positions.
            for (int i = 0; i < this.blobInfos.Length; i++)
            {
                // Get element from target position.
                if (context.TryGetElementIndex(this.blobInfos[i].Position, context.CurrentLayer, out ElementIndex targetElement))
                {
                    // Check conditions for addition to blob value. If you fail, just continue to the next iteration.
                    if (targetElement != this.Element.Index)
                    {
                        continue;
                    }

                    // Upon successful completion of the conditions and steps, add to the blob value.
                    result += this.blobInfos[i].BlobValue;
                }
            }

            return result;
        }

        private void GetTargetPositionsFromIndex(int index, Point position)
        {
            switch (index)
            {
                // Sprite Piece 1 (Northwest Pivot)
                case 0:
                    this.blobInfos[0] = new(new(position.X - 1, position.Y), (byte)BlobDirection.West);
                    this.blobInfos[1] = new(new(position.X - 1, position.Y - 1), (byte)BlobDirection.Northwest);
                    this.blobInfos[2] = new(new(position.X, position.Y - 1), (byte)BlobDirection.North);
                    break;

                // Sprite Piece 2 (Northeast Pivot)
                case 1:
                    this.blobInfos[0] = new(new(position.X + 1, position.Y), (byte)BlobDirection.East);
                    this.blobInfos[1] = new(new(position.X + 1, position.Y - 1), (byte)BlobDirection.Northeast);
                    this.blobInfos[2] = new(new(position.X, position.Y - 1), (byte)BlobDirection.North);
                    break;

                // Sprite Piece 3 (Southwest Pivot)
                case 2:
                    this.blobInfos[0] = new(new(position.X - 1, position.Y), (byte)BlobDirection.West);
                    this.blobInfos[1] = new(new(position.X - 1, position.Y + 1), (byte)BlobDirection.Southwest);
                    this.blobInfos[2] = new(new(position.X, position.Y + 1), (byte)BlobDirection.South);
                    break;

                // Sprite Piece 4 (Southeast Pivot)
                case 3:
                    this.blobInfos[0] = new(new(position.X + 1, position.Y), (byte)BlobDirection.East);
                    this.blobInfos[1] = new(new(position.X + 1, position.Y + 1), (byte)BlobDirection.Southeast);
                    this.blobInfos[2] = new(new(position.X, position.Y + 1), (byte)BlobDirection.South);
                    break;

                default:
                    this.blobInfos[0] = default;
                    this.blobInfos[1] = default;
                    this.blobInfos[2] = default;
                    break;
            }
        }

        private void SetChunkSpriteFromIndexAndBlobValue(int index, in byte blobValue)
        {
            switch (index)
            {
                // (Sprite 1 - Northwest Pivot)
                case 0:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        ElementConstants.BLOB_NORTHWEST_PIVOT_EMPTY => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Corner_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_1 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Horizontal_Edge_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_2 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Corner_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_3 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Vertical_Edge_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_4 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Horizontal_Edge_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_5 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Vertical_Edge_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_CASE_6 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Gap_Northwest],
                        ElementConstants.BLOB_NORTHWEST_PIVOT_SURROUNDED => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Full_Northwest],
                        _ => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Full_Northwest],
                    };
                    break;

                // (Sprite 2 - Northeast Pivot)
                case 1:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        ElementConstants.BLOB_NORTHEAST_PIVOT_EMPTY => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Corner_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_1 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Horizontal_Edge_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_2 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Corner_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_3 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Vertical_Edge_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_4 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Horizontal_Edge_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_5 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Vertical_Edge_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_CASE_6 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Gap_Northeast],
                        ElementConstants.BLOB_NORTHEAST_PIVOT_SURROUNDED => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Full_Northeast],
                        _ => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Full_Northeast],
                    };
                    break;

                // (Sprite 3 - Southwest Pivot)
                case 2:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_EMPTY => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Corner_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_1 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Horizontal_Edge_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_2 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Corner_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_3 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Vertical_Edge_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_4 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Horizontal_Edge_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_5 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Vertical_Edge_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_CASE_6 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Gap_Southwest],
                        ElementConstants.BLOB_SOUTHWEST_PIVOT_SURROUNDED => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Full_Southwest],
                        _ => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Full_Southwest],
                    };
                    break;

                // (Sprite 4 - Southeast Pivot)
                case 3:
                    this.spriteClipAreas[index] = blobValue switch
                    {
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_EMPTY => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Corner_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_1 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Horizontal_Edge_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_2 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Corner_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_3 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Vertical_Edge_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_4 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Horizontal_Edge_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_5 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Vertical_Edge_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_CASE_6 => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Gap_Southeast],
                        ElementConstants.BLOB_SOUTHEAST_PIVOT_SURROUNDED => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Full_Southeast],
                        _ => ElementConstants.BLOB_SPRITE_KEY_POINTS[(int)SpriteKeyPoints.Full_Southeast],
                    };
                    break;

                default:
                    break;
            }
        }

        protected override void OnDraw(ElementContext context, SpriteBatch spriteBatch)
        {
            SlotLayer slotLayer = context.CurrentSlot.GetLayer(context.CurrentLayer);
            Color colorModifier = slotLayer.ColorModifier;

            if (this.GameplaySettings.ShowTemperatureColorVariations)
            {
                colorModifier = TemperatureConstants.ApplyHeatColor(slotLayer.ColorModifier, slotLayer.Temperature);
            }

            if (context.CurrentLayer == Layer.Background)
            {
                colorModifier = colorModifier.Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            UpdateSpritePositions(context.CurrentSlot.Position);

            for (int i = 0; i < ElementConstants.SPRITE_DIVISIONS_LENGTH; i++)
            {
                UpdateSpriteSlice(context, i, context.CurrentSlot.Position);
                spriteBatch.Draw(this.AssetDatabase.GetTexture(TextureIndex.Elements), this.spritePositions[i], new(this.RenderingProfile.TextureOriginOffset + this.spriteClipAreas[i].Location, this.spriteClipAreas[i].Size), colorModifier, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
            }
        }
    }
}
