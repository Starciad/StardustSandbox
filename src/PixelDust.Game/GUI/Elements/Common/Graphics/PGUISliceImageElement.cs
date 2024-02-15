using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants.GUI.Elements;
using PixelDust.Game.Enums.General;

namespace PixelDust.Game.GUI.Elements.Common.Graphics
{
    public sealed class PGUISliceImageElement : PGUIGraphicElement
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

        private Texture2D texture = null;
        private Color color = Color.White;

        private readonly SliceInfo[] textureSlices = new SliceInfo[9];

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.texture == null)
            {
                return;
            }

            Point sizePoint = new(PSliceImageConstants.SPRITE_SLICE_SIZE);

            // Center
            this.textureSlices[(int)PCardinalDirection.Center].SetPosition(this.Position);
            this.textureSlices[(int)PCardinalDirection.Center].SetTextureClipArea(new Rectangle(new Point(PSliceImageConstants.SPRITE_SLICE_SIZE, PSliceImageConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Center].SetScale(this.Style.Size.ToVector2());

            // North
            this.textureSlices[(int)PCardinalDirection.North].SetPosition(new Vector2(this.Position.X, this.Position.Y - PSliceImageConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(int)PCardinalDirection.North].SetTextureClipArea(new Rectangle(new Point(PSliceImageConstants.SPRITE_SLICE_SIZE, 0), sizePoint));
            this.textureSlices[(int)PCardinalDirection.North].SetScale(new Vector2(this.Style.Size.Width, 1));

            // Northeast
            this.textureSlices[(int)PCardinalDirection.Northeast].SetPosition(new Vector2(this.Position.X + (PSliceImageConstants.SPRITE_SLICE_SIZE * this.Style.Size.Width), this.Position.Y - PSliceImageConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(int)PCardinalDirection.Northeast].SetTextureClipArea(new Rectangle(new Point(PSliceImageConstants.SPRITE_SLICE_SIZE * 2, 0), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Northeast].SetScale(Vector2.One);

            // East
            this.textureSlices[(int)PCardinalDirection.East].SetPosition(new Vector2(this.Position.X + (PSliceImageConstants.SPRITE_SLICE_SIZE * this.Style.Size.Width), this.Position.Y));
            this.textureSlices[(int)PCardinalDirection.East].SetTextureClipArea(new Rectangle(new Point(PSliceImageConstants.SPRITE_SLICE_SIZE * 2, PSliceImageConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(int)PCardinalDirection.East].SetScale(new Vector2(1, this.Style.Size.Height));

            // Southeast
            this.textureSlices[(int)PCardinalDirection.Southeast].SetPosition(new Vector2(this.Position.X + (PSliceImageConstants.SPRITE_SLICE_SIZE * this.Style.Size.Width), this.Position.Y + (PSliceImageConstants.SPRITE_SLICE_SIZE * this.Style.Size.Height)));
            this.textureSlices[(int)PCardinalDirection.Southeast].SetTextureClipArea(new Rectangle(new Point(PSliceImageConstants.SPRITE_SLICE_SIZE * 2, PSliceImageConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Southeast].SetScale(Vector2.One);

            // South
            this.textureSlices[(int)PCardinalDirection.South].SetPosition(new Vector2(this.Position.X, this.Position.Y + (PSliceImageConstants.SPRITE_SLICE_SIZE * this.Style.Size.Height)));
            this.textureSlices[(int)PCardinalDirection.South].SetTextureClipArea(new Rectangle(new Point(PSliceImageConstants.SPRITE_SLICE_SIZE, PSliceImageConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)PCardinalDirection.South].SetScale(new Vector2(this.Style.Size.Width, 1));

            // Southwest
            this.textureSlices[(int)PCardinalDirection.Southwest].SetPosition(new Vector2(this.Position.X - PSliceImageConstants.SPRITE_SLICE_SIZE, this.Position.Y + (PSliceImageConstants.SPRITE_SLICE_SIZE * this.Style.Size.Height)));
            this.textureSlices[(int)PCardinalDirection.Southwest].SetTextureClipArea(new Rectangle(new Point(0, PSliceImageConstants.SPRITE_SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Southwest].SetScale(Vector2.One);

            // West
            this.textureSlices[(int)PCardinalDirection.West].SetPosition(new Vector2(this.Position.X - PSliceImageConstants.SPRITE_SLICE_SIZE, this.Position.Y));
            this.textureSlices[(int)PCardinalDirection.West].SetTextureClipArea(new Rectangle(new Point(0, PSliceImageConstants.SPRITE_SLICE_SIZE), sizePoint));
            this.textureSlices[(int)PCardinalDirection.West].SetScale(new Vector2(1, this.Style.Size.Height));

            // Northwest
            this.textureSlices[(int)PCardinalDirection.Northwest].SetPosition(new Vector2(this.Position.X - PSliceImageConstants.SPRITE_SLICE_SIZE, this.Position.Y - PSliceImageConstants.SPRITE_SLICE_SIZE));
            this.textureSlices[(int)PCardinalDirection.Northwest].SetTextureClipArea(new Rectangle(new Point(0, 0), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Northwest].SetScale(Vector2.One);
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
    }
}
