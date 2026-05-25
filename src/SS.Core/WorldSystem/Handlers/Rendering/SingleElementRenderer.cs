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
    internal sealed class SingleElementRenderer : ElementRenderer
    {
        internal SingleElementRenderer(AssetDatabase assetDatabase, Camera2D camera, GameplaySettings gameplaySettings) : base(assetDatabase, camera, gameplaySettings)
        {

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

            spriteBatch.Draw(this.AssetDatabase.GetTexture(TextureIndex.Elements), new Vector2(context.CurrentSlot.Position.X, context.CurrentSlot.Position.Y) * WorldConstants.TILE_SIZE, new(this.RenderingProfile.TextureOriginOffset, new(32)), colorModifier, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
