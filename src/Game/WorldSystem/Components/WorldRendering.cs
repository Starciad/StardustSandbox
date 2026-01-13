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

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Core;
using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.World;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;

namespace StardustSandbox.WorldSystem.Components
{
    internal sealed class WorldRendering(InputController inputController, World world)
    {
        internal bool DrawForegroundElements { get; set; } = true;
        internal bool DrawBackgroundElements { get; set; } = true;

        private readonly ElementContext elementRenderingContext = new(world);
        private readonly InputController inputController = inputController;
        private readonly World world = world;

        internal void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftWorld = SSCamera.ScreenToWorld(new(0, 0));
            Vector2 bottomRightWorld = SSCamera.ScreenToWorld(new(ScreenConstants.SCREEN_DIMENSIONS.X, ScreenConstants.SCREEN_DIMENSIONS.Y));

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

                    if (this.world.TryGetSlot(targetPosition.ToPoint(), out Slot value))
                    {
                        if (this.DrawBackgroundElements && !value.Background.IsEmpty)
                        {
                            DrawSlotLayer(spriteBatch, value.Position, Layer.Background, value, value.GetLayer(Layer.Background).Element, gameplaySettings);
                        }

                        if (this.DrawForegroundElements && !value.Foreground.IsEmpty)
                        {
                            DrawSlotLayer(spriteBatch, value.Position, Layer.Foreground, value, value.GetLayer(Layer.Foreground).Element, gameplaySettings);
                        }
                    }
                }
            }
        }

        private void DrawSlotLayer(SpriteBatch spriteBatch, in Point position, in Layer layer, Slot slot, Element element, in GameplaySettings gameplaySettings)
        {
            this.elementRenderingContext.UpdateInformation(position, layer, slot);

            ElementRenderer.Draw(this.elementRenderingContext, element, spriteBatch, element.TextureOriginOffset, gameplaySettings);
        }
    }
}

