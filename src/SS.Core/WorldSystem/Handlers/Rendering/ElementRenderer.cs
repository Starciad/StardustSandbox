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
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.WorldSystem.Handlers.Rendering
{
    internal abstract class ElementRenderer
    {
        protected Element Element { get; private set; }
        protected ElementRenderingProfile RenderingProfile { get; private set; }

        protected AssetDatabase AssetDatabase { get; }
        protected Camera2D Camera { get; }
        protected GameplaySettings GameplaySettings { get; }

        internal ElementRenderer(AssetDatabase assetDatabase, Camera2D camera, GameplaySettings gameplaySettings)
        {
            this.AssetDatabase = assetDatabase;
            this.Camera = camera;
            this.GameplaySettings = gameplaySettings;
        }

        private void DrawPixelElementRoutine(ElementContext context, SpriteBatch spriteBatch)
        {
            SlotLayer slotLayer = context.CurrentSlot.GetLayer(context.CurrentLayer);

            Color referenceColor = slotLayer.Element.RenderingProfile.ReferenceColor;
            Color colorModifier = slotLayer.ColorModifier;

            if (this.GameplaySettings.ShowTemperatureColorVariations)
            {
                colorModifier = TemperatureConstants.ApplyHeatColor(colorModifier, slotLayer.Temperature);
            }

            if (context.CurrentLayer == Layer.Background)
            {
                colorModifier = colorModifier.Darken(WorldConstants.BACKGROUND_COLOR_DARKENING_FACTOR);
            }

            Color finalColor = new(
                (byte)(referenceColor.R * colorModifier.R / 255),
                (byte)(referenceColor.G * colorModifier.G / 255),
                (byte)(referenceColor.B * colorModifier.B / 255),
                referenceColor.A
            );

            spriteBatch.Draw(this.AssetDatabase.GetTexture(TextureIndex.Pixel), new Vector2(context.CurrentSlot.Position.X, context.CurrentSlot.Position.Y) * WorldConstants.TILE_SIZE, null, finalColor, 0f, Vector2.Zero, new Vector2(WorldConstants.TILE_SIZE), SpriteEffects.None, 0f);
        }

        internal void Draw(ElementContext context, SpriteBatch spriteBatch)
        {
            // If the camera is too far away, draw only a single pixel
            // that can represent the element to aid in performance and
            // visibility.
            if (this.Camera.Zoom <= CameraConstants.PIXEL_RENDER_ZOOM_THRESHOLD)
            {
                DrawPixelElementRoutine(context, spriteBatch);
                return;
            }

            this.Element = context.CurrentSlotLayer.Element;
            this.RenderingProfile = this.Element.RenderingProfile;

            OnDraw(context, spriteBatch);
        }

        protected abstract void OnDraw(ElementContext context, SpriteBatch spriteBatch);
    }
}
