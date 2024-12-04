using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.GUI.Elements;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.GUISystem.Elements.Graphics
{
    public sealed class SGUISliceImageElement : SGUIGraphicElement
    {
        private struct SliceInfo()
        {
            public Rectangle TextureClipArea { get; private set; }
            public Vector2 Position { get; private set; }
            public Vector2 Scale { get; private set; }

            public void SetTextureClipArea(Rectangle value)
            {
                this.TextureClipArea = value;
            }

            public void SetPosition(Vector2 value)
            {
                this.Position = value;
            }

            public void SetScale(Vector2 value)
            {
                this.Scale = value;
            }
        }

        private readonly SliceInfo[] textureSlices = new SliceInfo[9];

        public SGUISliceImageElement(ISGame gameInstance) : base(gameInstance)
        {
            this.ShouldUpdate = true;
            this.IsVisible = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Texture == null)
            {
                return;
            }

            Point sizePoint = new(SSliceImageConstants.SPRITE_SLICE_SIZE);

            // Center
            this.textureSlices[(int)SCardinalDirection.Center].SetPosition(this.Position);
            this.textureSlices[(int)SCardinalDirection.Center].SetTextureClipArea(new Rectangle(new Point(SSliceImageConstants.SPRITE_SLICE_SIZE, SSliceImageConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(int)SCardinalDirection.Center].SetScale(this.Scale);

            // North
            this.textureSlices[(int)SCardinalDirection.North].SetPosition(new Vector2(this.Position.X, this.Position.Y - SSliceImageConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(int)SCardinalDirection.North].SetTextureClipArea(new Rectangle(new Point(SSliceImageConstants.SPRITE_SLICE_SIZE, 0), sizePoint));
            this.textureSlices[(int)SCardinalDirection.North].SetScale(new Vector2(this.Scale.X, 1));

            // Northeast
            this.textureSlices[(int)SCardinalDirection.Northeast].SetPosition(new Vector2(this.Position.X + (SSliceImageConstants.SPRITE_SLICE_SIZE * this.Scale.X), this.Position.Y - SSliceImageConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(int)SCardinalDirection.Northeast].SetTextureClipArea(new Rectangle(new Point(SSliceImageConstants.SPRITE_SLICE_SIZE * 2, 0), sizePoint));
            this.textureSlices[(int)SCardinalDirection.Northeast].SetScale(Vector2.One);

            // East
            this.textureSlices[(int)SCardinalDirection.East].SetPosition(new Vector2(this.Position.X + (SSliceImageConstants.SPRITE_SLICE_SIZE * this.Scale.X), this.Position.Y));
            this.textureSlices[(int)SCardinalDirection.East].SetTextureClipArea(new Rectangle(new Point(SSliceImageConstants.SPRITE_SLICE_SIZE * 2, SSliceImageConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(int)SCardinalDirection.East].SetScale(new Vector2(1, this.Scale.Y));

            // Southeast
            this.textureSlices[(int)SCardinalDirection.Southeast].SetPosition(new Vector2(this.Position.X + (SSliceImageConstants.SPRITE_SLICE_SIZE * this.Scale.X), this.Position.Y + (SSliceImageConstants.SPRITE_SLICE_SIZE * this.Scale.Y)));
            this.textureSlices[(int)SCardinalDirection.Southeast].SetTextureClipArea(new Rectangle(new Point(SSliceImageConstants.SPRITE_SLICE_SIZE * 2, SSliceImageConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)SCardinalDirection.Southeast].SetScale(Vector2.One);

            // South
            this.textureSlices[(int)SCardinalDirection.South].SetPosition(new Vector2(this.Position.X, this.Position.Y + (SSliceImageConstants.SPRITE_SLICE_SIZE * this.Scale.Y)));
            this.textureSlices[(int)SCardinalDirection.South].SetTextureClipArea(new Rectangle(new Point(SSliceImageConstants.SPRITE_SLICE_SIZE, SSliceImageConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)SCardinalDirection.South].SetScale(new Vector2(this.Scale.X, 1));

            // Southwest
            this.textureSlices[(int)SCardinalDirection.Southwest].SetPosition(new Vector2(this.Position.X - SSliceImageConstants.SPRITE_SLICE_SIZE, this.Position.Y + (SSliceImageConstants.SPRITE_SLICE_SIZE * this.Scale.Y)));
            this.textureSlices[(int)SCardinalDirection.Southwest].SetTextureClipArea(new Rectangle(new Point(0, SSliceImageConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)SCardinalDirection.Southwest].SetScale(Vector2.One);

            // West
            this.textureSlices[(int)SCardinalDirection.West].SetPosition(new Vector2(this.Position.X - SSliceImageConstants.SPRITE_SLICE_SIZE, this.Position.Y));
            this.textureSlices[(int)SCardinalDirection.West].SetTextureClipArea(new Rectangle(new Point(0, SSliceImageConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(int)SCardinalDirection.West].SetScale(new Vector2(1, this.Scale.Y));

            // Northwest
            this.textureSlices[(int)SCardinalDirection.Northwest].SetPosition(new Vector2(this.Position.X - SSliceImageConstants.SPRITE_SLICE_SIZE, this.Position.Y - SSliceImageConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(int)SCardinalDirection.Northwest].SetTextureClipArea(new Rectangle(new Point(0, 0), sizePoint));
            this.textureSlices[(int)SCardinalDirection.Northwest].SetScale(Vector2.One);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
