using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private enum SliceIndex
        {
            Center = 0,
            North = 1,
            Northeast = 2,
            East = 3,
            Southeast = 4,
            South = 5,
            Southwest = 6,
            West = 7,
            Northwest = 8
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
            this.textureSlices[(int)SliceIndex.Center].SetPosition(this.Position);
            this.textureSlices[(int)SliceIndex.Center].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE, SLICE_SIZE), sizePoint));
            this.textureSlices[(int)SliceIndex.Center].SetScale(new Vector2(this.Style.Size.Width, this.Style.Size.Height));

            // North
            this.textureSlices[(int)SliceIndex.North].SetPosition(new Vector2(this.Position.X, this.Position.Y - SLICE_SIZE));
            this.textureSlices[(int)SliceIndex.North].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE, 0), sizePoint));
            this.textureSlices[(int)SliceIndex.North].SetScale(new Vector2(this.Style.Size.Width, 1));

            // Northeast
            this.textureSlices[(int)SliceIndex.Northeast].SetPosition(new Vector2(this.Position.X + (SLICE_SIZE * this.Style.Size.Width), this.Position.Y - SLICE_SIZE));
            this.textureSlices[(int)SliceIndex.Northeast].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE * 2, 0), sizePoint));
            this.textureSlices[(int)SliceIndex.Northeast].SetScale(Vector2.One);

            // East
            this.textureSlices[(int)SliceIndex.East].SetPosition(new Vector2(this.Position.X + SLICE_SIZE * this.Style.Size.Width, this.Position.Y));
            this.textureSlices[(int)SliceIndex.East].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE * 2, SLICE_SIZE), sizePoint));
            this.textureSlices[(int)SliceIndex.East].SetScale(new Vector2(1, this.Style.Size.Height));

            // Southeast
            this.textureSlices[(int)SliceIndex.Southeast].SetPosition(new Vector2(this.Position.X + SLICE_SIZE * this.Style.Size.Width, this.Position.Y + SLICE_SIZE * this.Style.Size.Height));
            this.textureSlices[(int)SliceIndex.Southeast].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE * 2, SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)SliceIndex.Southeast].SetScale(Vector2.One);

            // South
            this.textureSlices[(int)SliceIndex.South].SetPosition(new Vector2(this.Position.X, this.Position.Y + SLICE_SIZE * this.Style.Size.Height));
            this.textureSlices[(int)SliceIndex.South].SetTextureClipArea(new Rectangle(new Point(SLICE_SIZE, SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)SliceIndex.South].SetScale(new Vector2(this.Style.Size.Width, 1));

            // Southwest
            this.textureSlices[(int)SliceIndex.Southwest].SetPosition(new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y + SLICE_SIZE * this.Style.Size.Height));
            this.textureSlices[(int)SliceIndex.Southwest].SetTextureClipArea(new Rectangle(new Point(0, SLICE_SIZE * 2), sizePoint));
            this.textureSlices[(int)SliceIndex.Southwest].SetScale(Vector2.One);

            // West
            this.textureSlices[(int)SliceIndex.West].SetPosition(new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y));
            this.textureSlices[(int)SliceIndex.West].SetTextureClipArea(new Rectangle(new Point(0, SLICE_SIZE), sizePoint));
            this.textureSlices[(int)SliceIndex.West].SetScale(new Vector2(1, this.Style.Size.Height));

            // Northwest
            this.textureSlices[(int)SliceIndex.Northwest].SetPosition(new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y - SLICE_SIZE));
            this.textureSlices[(int)SliceIndex.Northwest].SetTextureClipArea(new Rectangle(new Point(0, 0), sizePoint));
            this.textureSlices[(int)SliceIndex.Northwest].SetScale(Vector2.One);
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
