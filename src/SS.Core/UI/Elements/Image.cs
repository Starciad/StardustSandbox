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

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class Image : UIElement
    {
        internal bool HasTexture => this.Texture != null;

        internal Texture2D Texture { get; set; }
        internal Rectangle? SourceRectangle { get; set; }
        internal Color Color { get; set; }

        public Image()
        {
            this.Color = Color.White;
        }

        public Image(Texture2D texture) : this()
        {
            this.Texture = texture;
        }

        public Image(Texture2D texture, Rectangle? sourceRectangle) : this(texture)
        {
            this.SourceRectangle = sourceRectangle;
        }

        internal void DisposeTexture()
        {
            this.Texture?.Dispose();
            this.Texture = null;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (this.HasTexture)
            {
                spriteBatch.Draw(this.Texture, this.Position, this.SourceRectangle, this.Color, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);
            }
        }
    }
}

