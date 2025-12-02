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

        internal override void Initialize()
        {
            int originX = this.Origin.X;
            int originY = this.Origin.Y;
            int tileWidth = this.TileSize.X;
            int tileHeight = this.TileSize.Y;

            // Center
            this.slices[(byte)CardinalDirection.Center].SetSourceRectangle(
                new(originX + tileWidth, originY + tileHeight, tileWidth, tileHeight)
            );

            // North
            this.slices[(byte)CardinalDirection.North].SetSourceRectangle(
                new(originX + tileWidth, originY, tileWidth, tileHeight)
            );

            // Northeast
            this.slices[(byte)CardinalDirection.Northeast].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY, tileWidth, tileHeight)
            );

            // East
            this.slices[(byte)CardinalDirection.East].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY + tileHeight, tileWidth, tileHeight)
            );

            // Southeast
            this.slices[(byte)CardinalDirection.Southeast].SetSourceRectangle(
                new(originX + (tileWidth * 2), originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // South
            this.slices[(byte)CardinalDirection.South].SetSourceRectangle(
                new(originX + tileWidth, originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // Southwest
            this.slices[(byte)CardinalDirection.Southwest].SetSourceRectangle(
                new(originX, originY + (tileHeight * 2), tileWidth, tileHeight)
            );

            // West
            this.slices[(byte)CardinalDirection.West].SetSourceRectangle(
                new(originX, originY + tileHeight, tileWidth, tileHeight)
            );

            // Northwest
            this.slices[(byte)CardinalDirection.Northwest].SetSourceRectangle(
                new(originX, originY, tileWidth, tileHeight)
            );

            base.Initialize();
        }

        internal override void Update(GameTime gameTime)
        {
            if (!this.HasTexture)
            {
                base.Update(gameTime);
                return;
            }

            int tileWidth = this.TileSize.X;
            int tileHeight = this.TileSize.Y;

            // Center
            this.slices[(byte)CardinalDirection.Center].SetPosition(this.Position);
            this.slices[(byte)CardinalDirection.Center].SetScale(this.TileScale);

            // North
            this.slices[(byte)CardinalDirection.North].SetPosition(new(this.Position.X, this.Position.Y - tileHeight));
            this.slices[(byte)CardinalDirection.North].SetScale(new(this.TileScale.X, 1));

            // Northeast
            this.slices[(byte)CardinalDirection.Northeast].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y - tileHeight));
            this.slices[(byte)CardinalDirection.Northeast].SetScale(Vector2.One);

            // East
            this.slices[(byte)CardinalDirection.East].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y));
            this.slices[(byte)CardinalDirection.East].SetScale(new(1, this.TileScale.Y));

            // Southeast
            this.slices[(byte)CardinalDirection.Southeast].SetPosition(new(this.Position.X + (tileWidth * this.TileScale.X), this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)CardinalDirection.Southeast].SetScale(Vector2.One);

            // South
            this.slices[(byte)CardinalDirection.South].SetPosition(new(this.Position.X, this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)CardinalDirection.South].SetScale(new(this.TileScale.X, 1));

            // Southwest
            this.slices[(byte)CardinalDirection.Southwest].SetPosition(new(this.Position.X - tileWidth, this.Position.Y + (tileHeight * this.TileScale.Y)));
            this.slices[(byte)CardinalDirection.Southwest].SetScale(Vector2.One);

            // West
            this.slices[(byte)CardinalDirection.West].SetPosition(new(this.Position.X - tileWidth, this.Position.Y));
            this.slices[(byte)CardinalDirection.West].SetScale(new(1, this.TileScale.Y));

            // Northwest
            this.slices[(byte)CardinalDirection.Northwest].SetPosition(new(this.Position.X - tileWidth, this.Position.Y - tileHeight));
            this.slices[(byte)CardinalDirection.Northwest].SetScale(Vector2.One);

            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (!this.HasTexture)
            {
                base.Draw(spriteBatch);
                return;
            }

            for (int i = 0; i < this.slices.Length; i++)
            {
                spriteBatch.Draw(this.Texture, this.slices[i].Position, this.slices[i].SourceRectangle, this.Color, 0f, Vector2.Zero, this.slices[i].Scale, SpriteEffects.None, 0f);
            }

            base.Draw(spriteBatch);
        }
    }
}
