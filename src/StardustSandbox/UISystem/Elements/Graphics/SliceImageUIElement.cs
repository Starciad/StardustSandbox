using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Directions;

namespace StardustSandbox.UISystem.Elements.Graphics
{
    internal sealed class SliceImageUIElement : GraphicUIElement
    {
        private struct SliceInfo()
        {
            internal Rectangle TextureClipArea { get; private set; }
            internal Vector2 Position { get; private set; }
            internal Vector2 Scale { get; private set; }

            internal void SetTextureClipArea(Rectangle value)
            {
                this.TextureClipArea = value;
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

        private readonly SliceInfo[] textureSlices = new SliceInfo[9];

        internal SliceImageUIElement()
        {
            this.ShouldUpdate = true;
            this.IsVisible = true;

            Point sizePoint = new(UIConstants.SPRITE_SLICE_SIZE);

            this.textureSlices[(byte)CardinalDirection.Center].SetTextureClipArea(new Rectangle(new Point(UIConstants.SPRITE_SLICE_SIZE, UIConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(byte)CardinalDirection.North].SetTextureClipArea(new Rectangle(new Point(UIConstants.SPRITE_SLICE_SIZE, 0), sizePoint));
            this.textureSlices[(byte)CardinalDirection.Northeast].SetTextureClipArea(new Rectangle(new Point(UIConstants.SPRITE_SLICE_SIZE * 2, 0), sizePoint));
            this.textureSlices[(byte)CardinalDirection.East].SetTextureClipArea(new Rectangle(new Point(UIConstants.SPRITE_SLICE_SIZE * 2, UIConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(byte)CardinalDirection.Southeast].SetTextureClipArea(new Rectangle(new Point(UIConstants.SPRITE_SLICE_SIZE * 2, UIConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(byte)CardinalDirection.South].SetTextureClipArea(new Rectangle(new Point(UIConstants.SPRITE_SLICE_SIZE, UIConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(byte)CardinalDirection.Southwest].SetTextureClipArea(new Rectangle(new Point(0, UIConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(byte)CardinalDirection.West].SetTextureClipArea(new Rectangle(new Point(0, UIConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(byte)CardinalDirection.Northwest].SetTextureClipArea(new Rectangle(new Point(0, 0), sizePoint));
        }

        internal override void Initialize()
        {
            return;
        }

        internal override void Update(GameTime gameTime)
        {
            if (this.Texture == null)
            {
                return;
            }

            // Center
            this.textureSlices[(byte)CardinalDirection.Center].SetPosition(this.Position);
            this.textureSlices[(byte)CardinalDirection.Center].SetScale(this.Scale);

            // North
            this.textureSlices[(byte)CardinalDirection.North].SetPosition(new(this.Position.X, this.Position.Y - UIConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(byte)CardinalDirection.North].SetScale(new(this.Scale.X, 1));

            // Northeast
            this.textureSlices[(byte)CardinalDirection.Northeast].SetPosition(new(this.Position.X + (UIConstants.SPRITE_SLICE_SIZE * this.Scale.X), this.Position.Y - UIConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(byte)CardinalDirection.Northeast].SetScale(Vector2.One);

            // East
            this.textureSlices[(byte)CardinalDirection.East].SetPosition(new(this.Position.X + (UIConstants.SPRITE_SLICE_SIZE * this.Scale.X), this.Position.Y));
            this.textureSlices[(byte)CardinalDirection.East].SetScale(new(1, this.Scale.Y));

            // Southeast
            this.textureSlices[(byte)CardinalDirection.Southeast].SetPosition(new(this.Position.X + (UIConstants.SPRITE_SLICE_SIZE * this.Scale.X), this.Position.Y + (UIConstants.SPRITE_SLICE_SIZE * this.Scale.Y)));
            this.textureSlices[(byte)CardinalDirection.Southeast].SetScale(Vector2.One);

            // South
            this.textureSlices[(byte)CardinalDirection.South].SetPosition(new(this.Position.X, this.Position.Y + (UIConstants.SPRITE_SLICE_SIZE * this.Scale.Y)));
            this.textureSlices[(byte)CardinalDirection.South].SetScale(new(this.Scale.X, 1));

            // Southwest
            this.textureSlices[(byte)CardinalDirection.Southwest].SetPosition(new(this.Position.X - UIConstants.SPRITE_SLICE_SIZE, this.Position.Y + (UIConstants.SPRITE_SLICE_SIZE * this.Scale.Y)));
            this.textureSlices[(byte)CardinalDirection.Southwest].SetScale(Vector2.One);

            // West
            this.textureSlices[(byte)CardinalDirection.West].SetPosition(new(this.Position.X - UIConstants.SPRITE_SLICE_SIZE, this.Position.Y));
            this.textureSlices[(byte)CardinalDirection.West].SetScale(new(1, this.Scale.Y));

            // Northwest
            this.textureSlices[(byte)CardinalDirection.Northwest].SetPosition(new(this.Position.X - UIConstants.SPRITE_SLICE_SIZE, this.Position.Y - UIConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(byte)CardinalDirection.Northwest].SetScale(Vector2.One);
        }
        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Texture == null)
            {
                return;
            }

            for (int i = 0; i < this.textureSlices.Length; i++)
            {
                SliceInfo texturePiece = this.textureSlices[i];
                spriteBatch.Draw(this.Texture, texturePiece.Position, texturePiece.TextureClipArea, this.Color, 0f, Vector2.Zero, texturePiece.Scale, SpriteEffects.None, 0f);
            }
        }
    }
}
