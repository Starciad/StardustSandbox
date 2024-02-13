using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Enums.General;
using PixelDust.Game.Extensions;

using System.Collections.Generic;

namespace PixelDust.Game.GUI.Elements.Common
{
    public sealed class PGUISliceImageElement : PGUIElement
    {
        private struct SliceInfo
        {
            public Rectangle TextureClipArea { get; set; }
            public Vector2 Position { get; set; }
            public Vector2 Scale { get; set; }
            public Color Color { get; set; }
        }

        public Texture2D Texture => this.texture;

        private Texture2D texture;
        private readonly SliceInfo[] textureSlices = new SliceInfo[9];

        private const int SLICE_SIZE = 16;

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
            this.textureSlices[0].Position = this.Position;
            this.textureSlices[0].TextureClipArea = new Rectangle(new Point(SLICE_SIZE, SLICE_SIZE), sizePoint);
            this.textureSlices[0].Scale = new Vector2(this.Style.Size.Width, this.Style.Size.Height);
            this.textureSlices[0].Color = this.Style.Color;

            // North
            this.textureSlices[1].Position = new Vector2(this.Position.X, this.Position.Y - SLICE_SIZE);
            this.textureSlices[1].TextureClipArea = new Rectangle(new Point(SLICE_SIZE, 0), sizePoint);
            this.textureSlices[1].Scale = new Vector2(this.Style.Size.Width, 1);
            this.textureSlices[1].Color = Color.Red;

            // Northeast
            this.textureSlices[2].Position = new Vector2(this.Position.X + (SLICE_SIZE * this.Style.Size.Width), this.Position.Y - SLICE_SIZE);
            this.textureSlices[2].TextureClipArea = new Rectangle(new Point(SLICE_SIZE * 2, 0), sizePoint);
            this.textureSlices[2].Scale = Vector2.One;
            this.textureSlices[2].Color = Color.Green;

            // East
            this.textureSlices[3].Position = new Vector2(this.Position.X + SLICE_SIZE * this.Style.Size.Width, this.Position.Y);
            this.textureSlices[3].TextureClipArea = new Rectangle(new Point(SLICE_SIZE * 2, SLICE_SIZE), sizePoint);
            this.textureSlices[3].Scale = new Vector2(1, this.Style.Size.Height);
            this.textureSlices[3].Color = Color.Blue;

            // Southeast
            this.textureSlices[4].Position = new Vector2(this.Position.X + SLICE_SIZE * this.Style.Size.Width, this.Position.Y + SLICE_SIZE * this.Style.Size.Height);
            this.textureSlices[4].TextureClipArea = new Rectangle(new Point(SLICE_SIZE * 2, SLICE_SIZE * 2), sizePoint);
            this.textureSlices[4].Scale = Vector2.One;
            this.textureSlices[4].Color = Color.Yellow;

            // South
            this.textureSlices[5].Position = new Vector2(this.Position.X, this.Position.Y + SLICE_SIZE * this.Style.Size.Height);
            this.textureSlices[5].TextureClipArea = new Rectangle(new Point(SLICE_SIZE, SLICE_SIZE * 2), sizePoint);
            this.textureSlices[5].Scale = new Vector2(this.Style.Size.Width, 1);
            this.textureSlices[5].Color = Color.Brown;

            // Southwest
            this.textureSlices[6].Position = new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y + SLICE_SIZE * this.Style.Size.Height);
            this.textureSlices[6].TextureClipArea = new Rectangle(new Point(0, SLICE_SIZE * 2), sizePoint);
            this.textureSlices[6].Scale = Vector2.One;
            this.textureSlices[6].Color = Color.Orange;

            // West
            this.textureSlices[7].Position = new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y);
            this.textureSlices[7].TextureClipArea = new Rectangle(new Point(0, SLICE_SIZE), sizePoint);
            this.textureSlices[7].Scale = new Vector2(1, this.Style.Size.Height);
            this.textureSlices[7].Color = Color.Gray;

            // Northwest
            this.textureSlices[8].Position = new Vector2(this.Position.X - SLICE_SIZE, this.Position.Y - SLICE_SIZE);
            this.textureSlices[8].TextureClipArea = new Rectangle(new Point(0, 0), sizePoint);
            this.textureSlices[8].Scale = Vector2.One;
            this.textureSlices[8].Color = Color.Pink;
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.texture == null)
            {
                return;
            }

            foreach (SliceInfo texturePiece in this.textureSlices)
            {
                spriteBatch.Draw(this.texture, texturePiece.Position, texturePiece.TextureClipArea, texturePiece.Color, 0f, Vector2.Zero, texturePiece.Scale, SpriteEffects.None, 0f);
            }
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
