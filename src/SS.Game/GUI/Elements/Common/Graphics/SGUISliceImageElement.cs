using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants.GUI.Elements;
using StardustSandbox.Game.Enums.General;

namespace StardustSandbox.Game.GUI.Elements.Common.Graphics
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

        public override Texture2D Texture => this.texture;
        public override Color Color => this.color;
        public override Vector2 Scale => this.scale;

        private Texture2D texture = null;
        private Color color = Color.White;
        private Vector2 scale;

        private readonly SliceInfo[] textureSlices = new SliceInfo[9];

        public SGUISliceImageElement()
        {
            this.ShouldUpdate = true;
            this.IsVisible = true;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.texture == null)
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
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.texture == null)
            {
                return;
            }

            foreach (SliceInfo texturePiece in this.textureSlices)
            {
                spriteBatch.Draw(this.texture, texturePiece.Position, texturePiece.TextureClipArea, this.color, 0f, Vector2.Zero, texturePiece.Scale, SpriteEffects.None, 0f);
            }
        }

        public override void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public override void SetColor(Color color)
        {
            this.color = color;
        }

        public override void SetScale(Vector2 scale)
        {
            this.scale = scale;
        }
    }
}
