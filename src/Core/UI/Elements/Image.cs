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

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class Image : UIElement
    {
        internal bool HasTexture => this.texture != null;

        internal TextureIndex TextureIndex
        {
            get => this.textureIndex;
            set
            {
                this.textureIndex = value;
                this.texture = AssetDatabase.GetTexture(value);
            }
        }
        internal Texture2D Texture
        {
            get => this.texture;
            set
            {
                this.textureIndex = TextureIndex.None;
                this.texture = value;
            }
        }
        internal Rectangle? SourceRectangle
        {
            get => this.sourceRectangle;
            set => this.sourceRectangle = value;
        }
        internal Color Color
        {
            get => this.color;
            set => this.color = value;
        }

        private TextureIndex textureIndex;
        private Texture2D texture;
        private Color color;
        private Rectangle? sourceRectangle;

        internal Image()
        {
            this.CanDraw = true;
            this.CanUpdate = true;

            this.color = Color.White;
        }

        internal void DisposeTexture()
        {
            this.texture?.Dispose();
            this.texture = null;
        }

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            return;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (this.HasTexture)
            {
                spriteBatch.Draw(this.texture, this.Position, this.sourceRectangle, this.color, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);
            }
        }
    }
}

