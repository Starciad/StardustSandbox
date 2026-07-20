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

using StardustSandbox.Core.Enums.Directions;

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class SliceImageElement : UIElement
    {
        private struct SliceInfo()
        {
            internal Rectangle SourceRectangle { get; private set; }
            internal Vector2 Position { get; private set; }
            internal Vector2 Scale { get; private set; }

            internal void SetSourceRectangle(Rectangle value)
            {
                this.SourceRectangle = value;
            }

            internal void SetPosition(Vector2 value)
            {
                this.Position = value;
            }

            internal void SetScale(Vector2 value)
            {
                this.Scale = value;
            }
        }

        internal bool HasTexture => this.Texture != null;

        internal Texture2D Texture { get; set; }
        internal Point Origin { get; set; }
        internal Point TileSize { get; set; }
        internal Vector2 TileScale { get; set; }
        internal Color Color { get; set; }

        private readonly SliceInfo[] slices;

        public SliceImageElement()
        {
            this.TileSize = new(32);
            this.Color = Color.White;
            this.slices = new SliceInfo[9];
        }

        public SliceImageElement(Texture2D texture) : this()
        {
            this.Texture = texture;
        }

        protected override void OnInitialize()
        {
            int originX = this.Origin.X;
            int originY = this.Origin.Y;
            int tileWidth = this.TileSize.X;
            int tileHeight = this.TileSize.Y;

            // Center
            this.slices[(byte)UIAlignment.Center].SetSourceRectangle(
                new(originX + tileWidth, originY + tileHeight, tileWidth, tileHeight)
            );

            // North
            this.slices[(byte)UIAlignment.North].SetSourceRectangle(
                new(originX + tileWidth, originY, tileWidth, tileHeight)
            );

            // Northeast
            this.slices[(byte)UIAlignment.Northeast].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY, tileWidth, tileHeight)
            );

            // East
            this.slices[(byte)UIAlignment.East].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY + tileHeight, tileWidth, tileHeight)
            );

            // Southeast
            this.slices[(byte)UIAlignment.Southeast].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // South
            this.slices[(byte)UIAlignment.South].SetSourceRectangle(
                new(originX + tileWidth, originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // Southwest
            this.slices[(byte)UIAlignment.Southwest].SetSourceRectangle(
                new(originX, originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // West
            this.slices[(byte)UIAlignment.West].SetSourceRectangle(
                new(originX, originY + tileHeight, tileWidth, tileHeight)
            );

            // Northwest
            this.slices[(byte)UIAlignment.Northwest].SetSourceRectangle(
                new(originX, originY, tileWidth, tileHeight)
            );
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (!this.HasTexture)
            {
                return;
            }

            int tileWidth = this.TileSize.X;
            int tileHeight = this.TileSize.Y;

            // Center
            this.slices[(byte)UIAlignment.Center].SetPosition(this.Position);
            this.slices[(byte)UIAlignment.Center].SetScale(this.TileScale);

            // North
            this.slices[(byte)UIAlignment.North].SetPosition(new(this.Position.X, this.Position.Y - tileHeight));
            this.slices[(byte)UIAlignment.North].SetScale(new(this.TileScale.X, 1));

            // Northeast
            this.slices[(byte)UIAlignment.Northeast].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y - tileHeight));
            this.slices[(byte)UIAlignment.Northeast].SetScale(Vector2.One);

            // East
            this.slices[(byte)UIAlignment.East].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y));
            this.slices[(byte)UIAlignment.East].SetScale(new(1, this.TileScale.Y));

            // Southeast
            this.slices[(byte)UIAlignment.Southeast].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)UIAlignment.Southeast].SetScale(Vector2.One);

            // South
            this.slices[(byte)UIAlignment.South].SetPosition(new(this.Position.X, this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)UIAlignment.South].SetScale(new(this.TileScale.X, 1));

            // Southwest
            this.slices[(byte)UIAlignment.Southwest].SetPosition(new(this.Position.X - tileWidth, this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)UIAlignment.Southwest].SetScale(Vector2.One);

            // West
            this.slices[(byte)UIAlignment.West].SetPosition(new(this.Position.X - tileWidth, this.Position.Y));
            this.slices[(byte)UIAlignment.West].SetScale(new(1, this.TileScale.Y));

            // Northwest
            this.slices[(byte)UIAlignment.Northwest].SetPosition(new(this.Position.X - tileWidth, this.Position.Y - tileHeight));
            this.slices[(byte)UIAlignment.Northwest].SetScale(Vector2.One);
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (!this.HasTexture)
            {
                return;
            }

            for (int i = 0; i < this.slices.Length; i++)
            {
                spriteBatch.Draw(this.Texture, this.slices[i].Position, this.slices[i].SourceRectangle, this.Color, 0f, Vector2.Zero, this.slices[i].Scale, SpriteEffects.None, 0f);
            }
        }
    }
}

