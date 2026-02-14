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

namespace StardustSandbox.Core.Backgrounds
{
    internal sealed class Background
    {
        internal required BackgroundLayer[] Layers { get; init; }

        internal void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.Layers.Length; i++)
            {
                this.Layers[i].Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.Layers.Length; i++)
            {
                this.Layers[i].Draw(spriteBatch);
            }
        }
    }
}
