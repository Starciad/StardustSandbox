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

namespace StardustSandbox.Backgrounds
{
    internal sealed class Background
    {
        internal bool IsAffectedByLighting => this.isAffectedByLighting;

        private readonly BackgroundLayer[] backgroundLayers;
        private readonly bool isAffectedByLighting;
        private readonly int layerCount;
        private readonly Texture2D texture;

        internal Background(BackgroundLayer[] backgroundLayers, bool isAffectedByLighting, Texture2D texture)
        {
            this.backgroundLayers = backgroundLayers;
            this.isAffectedByLighting = isAffectedByLighting;
            this.layerCount = backgroundLayers.Length;
            this.texture = texture;
        }

        internal void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.layerCount; i++)
            {
                this.backgroundLayers[i].Update(gameTime, this.texture.Width, this.texture.Height);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.layerCount; i++)
            {
                this.backgroundLayers[i].Draw(spriteBatch, this.texture);
            }
        }
    }
}

