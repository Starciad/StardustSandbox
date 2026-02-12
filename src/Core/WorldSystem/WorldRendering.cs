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
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

using System;

namespace StardustSandbox.Core.WorldSystem
{
    internal sealed class WorldRendering(PlayerInputController inputController, World world)
    {
        internal bool DrawForegroundElements { get; set; } = true;
        internal bool DrawBackgroundElements { get; set; } = true;

        private readonly ElementContext elementRenderingContext = new(world);
        private readonly PlayerInputController inputController = inputController;
        private readonly World world = world;

        internal void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftWorld = Camera.ScreenToWorld(new(0, 0));
            Vector2 bottomRightWorld = Camera.ScreenToWorld(new(GameScreen.GetViewport().X, GameScreen.GetViewport().Y));

            int minTileX = (int)Math.Clamp(Math.Floor(topLeftWorld.X / WorldConstants.GRID_SIZE), 0, this.world.Information.Size.X);
            int minTileY = (int)Math.Clamp(Math.Floor(topLeftWorld.Y / WorldConstants.GRID_SIZE), 0, this.world.Information.Size.Y);
            int maxTileX = (int)Math.Clamp(Math.Ceiling(bottomRightWorld.X / WorldConstants.GRID_SIZE), 0, this.world.Information.Size.X);
            int maxTileY = (int)Math.Clamp(Math.Ceiling(bottomRightWorld.Y / WorldConstants.GRID_SIZE), 0, this.world.Information.Size.Y);

            GameplaySettings gameplaySettings = SettingsSerializer.Load<GameplaySettings>();

            for (int y = minTileY; y < maxTileY; y++)
            {
                for (int x = minTileX; x < maxTileX; x++)
                {
                    Vector2 targetPosition = new(x, y);

                    if (gameplaySettings.ShowGrid && this.inputController.Pen.Tool != PenTool.Visualization)
                    {
                        spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.ShapeSquares), targetPosition * WorldConstants.GRID_SIZE, new(32, 0, 32, 32), new(AAP64ColorPalette.White, gameplaySettings.GridOpacity), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    }

                    if (this.world.TryGetSlot(targetPosition.ToPoint(), out Slot slot))
                    {
                        if (this.DrawBackgroundElements && !slot.Background.IsEmpty)
                        {
                            DrawSlotLayer(spriteBatch, slot.Position, Layer.Background, slot.GetLayer(Layer.Background).Element, gameplaySettings);
                        }

                        if (this.DrawForegroundElements && !slot.Foreground.IsEmpty)
                        {
                            DrawSlotLayer(spriteBatch, slot.Position, Layer.Foreground, slot.GetLayer(Layer.Foreground).Element, gameplaySettings);
                        }
                    }
                }
            }
        }

        private void DrawSlotLayer(SpriteBatch spriteBatch, in Point position, in Layer layer, Element element, in GameplaySettings gameplaySettings)
        {
            this.elementRenderingContext.Initialize(position, layer);

            ElementRenderer.Draw(this.elementRenderingContext, element, spriteBatch, element.TextureOriginOffset, gameplaySettings);
        }
    }
}

