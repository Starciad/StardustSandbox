using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Directions;

namespace StardustSandbox.UI.Elements
{
    internal sealed class SliceImage : UIElement
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

        internal SliceImage()
        {
            this.CanDraw = true;
            this.CanUpdate = true;

            this.TileSize = new(32);
            this.Color = Color.White;

            this.slices = new SliceInfo[9];
        }

        protected override void OnInitialize()
        {
            int originX = this.Origin.X;
            int originY = this.Origin.Y;
            int tileWidth = this.TileSize.X;
            int tileHeight = this.TileSize.Y;

            // Center
            this.slices[(byte)UIDirection.Center].SetSourceRectangle(
                new(originX + tileWidth, originY + tileHeight, tileWidth, tileHeight)
            );

            // North
            this.slices[(byte)UIDirection.North].SetSourceRectangle(
                new(originX + tileWidth, originY, tileWidth, tileHeight)
            );

            // Northeast
            this.slices[(byte)UIDirection.Northeast].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY, tileWidth, tileHeight)
            );

            // East
            this.slices[(byte)UIDirection.East].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY + tileHeight, tileWidth, tileHeight)
            );

            // Southeast
            this.slices[(byte)UIDirection.Southeast].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // South
            this.slices[(byte)UIDirection.South].SetSourceRectangle(
                new(originX + tileWidth, originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // Southwest
            this.slices[(byte)UIDirection.Southwest].SetSourceRectangle(
                new(originX, originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // West
            this.slices[(byte)UIDirection.West].SetSourceRectangle(
                new(originX, originY + tileHeight, tileWidth, tileHeight)
            );

            // Northwest
            this.slices[(byte)UIDirection.Northwest].SetSourceRectangle(
                new(originX, originY, tileWidth, tileHeight)
            );
        }

        protected override void OnUpdate(in GameTime gameTime)
        {
            if (!this.HasTexture)
            {
                return;
            }

            int tileWidth = this.TileSize.X;
            int tileHeight = this.TileSize.Y;

            // Center
            this.slices[(byte)UIDirection.Center].SetPosition(this.Position);
            this.slices[(byte)UIDirection.Center].SetScale(this.TileScale);

            // North
            this.slices[(byte)UIDirection.North].SetPosition(new(this.Position.X, this.Position.Y - tileHeight));
            this.slices[(byte)UIDirection.North].SetScale(new(this.TileScale.X, 1));

            // Northeast
            this.slices[(byte)UIDirection.Northeast].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y - tileHeight));
            this.slices[(byte)UIDirection.Northeast].SetScale(Vector2.One);

            // East
            this.slices[(byte)UIDirection.East].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y));
            this.slices[(byte)UIDirection.East].SetScale(new(1, this.TileScale.Y));

            // Southeast
            this.slices[(byte)UIDirection.Southeast].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)UIDirection.Southeast].SetScale(Vector2.One);

            // South
            this.slices[(byte)UIDirection.South].SetPosition(new(this.Position.X, this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)UIDirection.South].SetScale(new(this.TileScale.X, 1));

            // Southwest
            this.slices[(byte)UIDirection.Southwest].SetPosition(new(this.Position.X - tileWidth, this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)UIDirection.Southwest].SetScale(Vector2.One);

            // West
            this.slices[(byte)UIDirection.West].SetPosition(new(this.Position.X - tileWidth, this.Position.Y));
            this.slices[(byte)UIDirection.West].SetScale(new(1, this.TileScale.Y));

            // Northwest
            this.slices[(byte)UIDirection.Northwest].SetPosition(new(this.Position.X - tileWidth, this.Position.Y - tileHeight));
            this.slices[(byte)UIDirection.Northwest].SetScale(Vector2.One);
        }

        protected override void OnDraw(in SpriteBatch spriteBatch)
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
