using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Enums.General;

namespace PixelDust.Game.GUI.Elements.Common
{
    public sealed class PGUISliceImageElement : PGUIElement
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

        public Texture2D Texture => this.texture;

        private const int SLICE_SIZE = 16;

        private Texture2D texture;
        private readonly SliceInfo[] textureSlices = new SliceInfo[9];

        public PGUISliceImageElement()
        {
            this.texture = null;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (this.texture == null)
            {
                return;
            }

            Point sizePoint = new(SLICE_SIZE);

            // Center
            this.textureSlices[(int)PCardinalDirection.Center].SetPosition(this.Position);
            this.textureSlices[(int)PCardinalDirection.Center].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE, SLICE_SIZE), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Center].SetScale(new Vector2(this.Style.Size.Width, this.Style.Size.Height));

            // North
            this.textureSlices[(int)PCardinalDirection.North].SetPosition(new Vector2(this.Position.X, this.Position.Y - SLICE_SIZE));
            this.textureSlices[(int)PCardinalDirection.North].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE, 0), sizePoint));
            this.textureSlices[(int)PCardinalDirection.North].SetScale(new Vector2(this.Style.Size.Width, 1));

            // Northeast
            this.textureSlices[(int)PCardinalDirection.Northeast].SetPosition(new Vector2(this.Position.X + (SLICE_SIZE * this.Style.Size.Width), this.Position.Y - SLICE_SIZE));
            this.textureSlices[(int)PCardinalDirection.Northeast].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE * 2, 0), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Northeast].SetScale(Vector2.One);

            // East
            this.textureSlices[(int)PCardinalDirection.East].SetPosition(new Vector2(this.Position.X + (SLICE_SIZE * this.Style.Size.Width), this.Position.Y));
            this.textureSlices[(int)PCardinalDirection.East].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE * 2, SLICE_SIZE), sizePoint));
            this.textureSlices[(int)PCardinalDirection.East].SetScale(new Vector2(1, this.Style.Size.Height));

            // Southeast
            this.textureSlices[(int)PCardinalDirection.Southeast].SetPosition(new Vector2(this.Position.X + (SLICE_SIZE * this.Style.Size.Width), this.Position.Y + (SLICE_SIZE * this.Style.Size.Height)));
            this.textureSlices[(int)PCardinalDirection.Southeast].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE * 2, SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Southeast].SetScale(Vector2.One);

            // South
            this.textureSlices[(int)PCardinalDirection.South].SetPosition(new Vector2(this.Position.X, this.Position.Y + (SLICE_SIZE * this.Style.Size.Height)));
            this.textureSlices[(int)PCardinalDirection.South].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE, SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)PCardinalDirection.South].SetScale(new Vector2(this.Style.Size.Width, 1));

            // Southwest
            this.textureSlices[(int)PCardinalDirection.Southwest].SetPosition(new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y + (SLICE_SIZE * this.Style.Size.Height)));
            this.textureSlices[(int)PCardinalDirection.Southwest].SetTextureClipArea(new Rectangle(new Point(0, SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)PCardinalDirection.Southwest].SetScale(Vector2.One);

            // West
            this.textureSlices[(int)PCardinalDirection.West].SetPosition(new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y));
            this.textureSlices[(int)PCardinalDirection.West].SetTextureClipArea(new Rectangle(new Point(0, SLICE_SIZE), sizePoint));
            this.textureSlices[(int)PCardinalDirection.West].SetScale(new Vector2(1, this.Style.Size.Height));

            // Northwest
            this.textureSlices[(int)PCardinalDirection.Northwest].SetPosition(new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y - SLICE_SIZE));
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
                spriteBatch.Draw(this.texture, texturePiece.Position, texturePiece.TextureClipArea, this.Style.Color, 0f, Vector2.Zero, texturePiece.Scale, SpriteEffects.None, 0f);
            }
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
