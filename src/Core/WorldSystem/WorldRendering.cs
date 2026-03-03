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
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

using System;

namespace StardustSandbox.Core.WorldSystem
{
    internal sealed class WorldRendering(PlayerInputController playerInputController, World world)
    {
        internal bool DrawForegroundElements { get; set; } = true;
        internal bool DrawBackgroundElements { get; set; } = true;

        private readonly ElementContext elementRenderingContext = new(world);
        private readonly PlayerInputController playerInputController = playerInputController;
        private readonly World world = world;

        internal void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            RectangleF viewBounds = camera.GetViewBounds();

            // Converts the visible world area to tile indexes
            int minTileX = (int)Math.Clamp(Math.Floor(viewBounds.Left / WorldConstants.TILE_SIZE), 0, this.world.Size.X);
            int minTileY = (int)Math.Clamp(Math.Floor(viewBounds.Top / WorldConstants.TILE_SIZE), 0, this.world.Size.Y);
            int maxTileX = (int)Math.Clamp(Math.Ceiling(viewBounds.Right / WorldConstants.TILE_SIZE), 0, this.world.Size.X);
            int maxTileY = (int)Math.Clamp(Math.Ceiling(viewBounds.Bottom / WorldConstants.TILE_SIZE), 0, this.world.Size.Y);

            GameplaySettings gameplaySettings = SettingsSerializer.Load<GameplaySettings>();

            for (int y = minTileY; y < maxTileY; y++)
            {
                for (int x = minTileX; x < maxTileX; x++)
                {
                    Vector2 targetPosition = new(x, y);

                    if (gameplaySettings.ShowGrid && this.playerInputController.Pen.Tool != PenTool.Visualization)
                    {
                        spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.ShapeSquares), targetPosition * WorldConstants.TILE_SIZE, new(32, 0, 32, 32), new(AAP64ColorPalette.White, gameplaySettings.GridOpacity), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    }

                    if (this.world.TryGetSlot(targetPosition.ToPoint(), out Slot slot))
                    {
                        if (this.DrawBackgroundElements && !slot.Background.IsEmpty)
                        {
                            DrawSlotLayer(spriteBatch, camera, slot.Position, Layer.Background, slot.GetLayer(Layer.Background).Element, gameplaySettings);
                        }

                        if (this.DrawForegroundElements && !slot.Foreground.IsEmpty)
                        {
                            DrawSlotLayer(spriteBatch, camera, slot.Position, Layer.Foreground, slot.GetLayer(Layer.Foreground).Element, gameplaySettings);
                        }
                    }
                }
            }
        }

        private void DrawSlotLayer(SpriteBatch spriteBatch, Camera2D camera, in Point position, in Layer layer, Element element, GameplaySettings gameplaySettings)
        {
            this.elementRenderingContext.Initialize(position, layer);

            ElementRenderer.Draw(this.elementRenderingContext, element, spriteBatch, camera, element.TextureOriginOffset, gameplaySettings);
        }
    }
}

