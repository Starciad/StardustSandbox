using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.UISystem.Elements
{
    internal sealed class SliceImage : UIElement
    {
        internal bool HasTexture => this.texture != null;
        internal bool HasSourceRectangle => this.sourceRectangle.HasValue;

        internal Texture2D Texture
        {
            get => this.texture;
            set
            {
                this.texture = value;
                RebuildSourceTiles();
                EnsureMinimumSize();
            }
        }

        /// <summary>
        /// Optional source rectangle in the texture that contains the 3x3 grid.
        /// If null, the whole texture is used.
        /// </summary>
        internal Rectangle? SourceRectangle
        {
            get => this.sourceRectangle;
            set
            {
                this.sourceRectangle = value;
                RebuildSourceTiles();
            }
        }

        /// <summary>
        /// Tile size in pixels inside the source rectangle. Default = 32.
        /// The source rectangle is expected to contain a 3x3 grid of tiles (so usually 3 * TileSize by 3 * TileSize).
        /// </summary>
        internal Point TileSize
        {
            get => this.tileSize;
            set
            {
                if (value.X <= 0 || value.Y <= 0)
                {
                    throw new ArgumentException("TileSize components must be > 0.");
                }

                this.tileSize = value;
                RebuildSourceTiles();
                EnsureMinimumSize();
            }
        }

        internal Color Color
        {
            get => this.color;
            set => this.color = value;
        }

        private Texture2D texture;
        private Rectangle? sourceRectangle;
        private Point tileSize = new(32, 32);
        private Color color = Color.White;

        // Indexing:
        // [0] top-left, [1] top-center, [2] top-right
        // [3] mid-left, [4] center,     [5] mid-right
        // [6] bot-left, [7] bot-center, [8] bot-right
        private readonly Rectangle[] sourceTiles = new Rectangle[9];

        internal SliceImage()
        {
            this.CanDraw = true;
            this.CanUpdate = true;

            for (int i = 0; i < this.sourceTiles.Length; i++)
            {
                this.sourceTiles[i] = Rectangle.Empty;
            }
        }

        internal override void Initialize()
        {
            base.Initialize();
            RebuildSourceTiles();
            EnsureMinimumSize();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (!this.HasTexture)
            {
                base.Draw(spriteBatch);
                return;
            }

            // final (destination) size in pixels including scale
            Vector2 finalSize = this.Size; // UIElement.Size returns rawSize * scale
            Vector2 pos = this.Position;

            // Compute how large corners should be on screen (corners scale with element scale)
            Vector2 scale = this.Scale;
            // corner sizes in destination coordinates (float)
            float cornerDestW = this.tileSize.X * scale.X;
            float cornerDestH = this.tileSize.Y * scale.Y;

            // Clamp final size to a minimum equal to two corners
            finalSize.X = MathF.Max(finalSize.X, cornerDestW * 2f);
            finalSize.Y = MathF.Max(finalSize.Y, cornerDestH * 2f);

            // center area (can be zero if the element is exactly the size of corners)
            float centerDestW = finalSize.X - (cornerDestW * 2f);
            float centerDestH = finalSize.Y - (cornerDestH * 2f);

            // Destination rectangles (rounded to ints)
            // Row 0 (top)
            Rectangle destTopLeft = new(
                (int)MathF.Round(pos.X),
                (int)MathF.Round(pos.Y),
                (int)MathF.Round(cornerDestW),
                (int)MathF.Round(cornerDestH));

            Rectangle destTopCenter = new(
                (int)MathF.Round(pos.X + cornerDestW),
                (int)MathF.Round(pos.Y),
                (int)MathF.Round(centerDestW),
                (int)MathF.Round(cornerDestH));

            Rectangle destTopRight = new(
                (int)MathF.Round(pos.X + cornerDestW + centerDestW),
                (int)MathF.Round(pos.Y),
                (int)MathF.Round(cornerDestW),
                (int)MathF.Round(cornerDestH));

            // Row 1 (middle)
            Rectangle destMidLeft = new(
                (int)MathF.Round(pos.X),
                (int)MathF.Round(pos.Y + cornerDestH),
                (int)MathF.Round(cornerDestW),
                (int)MathF.Round(centerDestH));

            Rectangle destCenter = new(
                (int)MathF.Round(pos.X + cornerDestW),
                (int)MathF.Round(pos.Y + cornerDestH),
                (int)MathF.Round(centerDestW),
                (int)MathF.Round(centerDestH));

            Rectangle destMidRight = new(
                (int)MathF.Round(pos.X + cornerDestW + centerDestW),
                (int)MathF.Round(pos.Y + cornerDestH),
                (int)MathF.Round(cornerDestW),
                (int)MathF.Round(centerDestH));

            // Row 2 (bottom)
            Rectangle destBotLeft = new(
                (int)MathF.Round(pos.X),
                (int)MathF.Round(pos.Y + cornerDestH + centerDestH),
                (int)MathF.Round(cornerDestW),
                (int)MathF.Round(cornerDestH));

            Rectangle destBotCenter = new(
                (int)MathF.Round(pos.X + cornerDestW),
                (int)MathF.Round(pos.Y + cornerDestH + centerDestH),
                (int)MathF.Round(centerDestW),
                (int)MathF.Round(cornerDestH));

            Rectangle destBotRight = new(
                (int)MathF.Round(pos.X + cornerDestW + centerDestW),
                (int)MathF.Round(pos.Y + cornerDestH + centerDestH),
                (int)MathF.Round(cornerDestW),
                (int)MathF.Round(cornerDestH));

            // Draw in a safe order
            // corners
            if (!this.sourceTiles[0].IsEmpty && destTopLeft.Width > 0 && destTopLeft.Height > 0)
            {
                spriteBatch.Draw(this.texture, destTopLeft, this.sourceTiles[0], this.color);
            }

            if (!this.sourceTiles[2].IsEmpty && destTopRight.Width > 0 && destTopRight.Height > 0)
            {
                spriteBatch.Draw(this.texture, destTopRight, this.sourceTiles[2], this.color);
            }

            if (!this.sourceTiles[6].IsEmpty && destBotLeft.Width > 0 && destBotLeft.Height > 0)
            {
                spriteBatch.Draw(this.texture, destBotLeft, this.sourceTiles[6], this.color);
            }

            if (!this.sourceTiles[8].IsEmpty && destBotRight.Width > 0 && destBotRight.Height > 0)
            {
                spriteBatch.Draw(this.texture, destBotRight, this.sourceTiles[8], this.color);
            }

            // edges (stretch along one axis)
            if (!this.sourceTiles[1].IsEmpty && destTopCenter.Width > 0 && destTopCenter.Height > 0)
            {
                spriteBatch.Draw(this.texture, destTopCenter, this.sourceTiles[1], this.color);
            }

            if (!this.sourceTiles[7].IsEmpty && destBotCenter.Width > 0 && destBotCenter.Height > 0)
            {
                spriteBatch.Draw(this.texture, destBotCenter, this.sourceTiles[7], this.color);
            }

            if (!this.sourceTiles[3].IsEmpty && destMidLeft.Width > 0 && destMidLeft.Height > 0)
            {
                spriteBatch.Draw(this.texture, destMidLeft, this.sourceTiles[3], this.color);
            }

            if (!this.sourceTiles[5].IsEmpty && destMidRight.Width > 0 && destMidRight.Height > 0)
            {
                spriteBatch.Draw(this.texture, destMidRight, this.sourceTiles[5], this.color);
            }

            // center (stretched both axes)
            if (!this.sourceTiles[4].IsEmpty && destCenter.Width > 0 && destCenter.Height > 0)
            {
                spriteBatch.Draw(this.texture, destCenter, this.sourceTiles[4], this.color);
            }

            base.Draw(spriteBatch);
        }

        internal void SetPixelSize(int width, int height)
        {
            Vector2 raw = new(width, height);
            raw = ClampToMinimumRawSize(raw);
            this.Size = raw;
        }

        private void RebuildSourceTiles()
        {
            if (this.texture == null)
            {
                for (int i = 0; i < this.sourceTiles.Length; i++)
                {
                    this.sourceTiles[i] = Rectangle.Empty;
                }

                return;
            }

            Rectangle baseSrc = this.sourceRectangle ?? new Rectangle(0, 0, this.texture.Width, this.texture.Height);

            // Ensure baseSrc is at least large enough to hold 3x3 tiles. If not, create the best-fit tiles (they may be truncated).
            // We'll compute tile offsets using tileSize.
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int index = (row * 3) + col;
                    int sx = baseSrc.X + (col * this.tileSize.X);
                    int sy = baseSrc.Y + (row * this.tileSize.Y);

                    // Clamp width/height to remain inside baseSrc bounds
                    int sw = Math.Min(this.tileSize.X, baseSrc.Right - sx);
                    int sh = Math.Min(this.tileSize.Y, baseSrc.Bottom - sy);

                    this.sourceTiles[index] = sw <= 0 || sh <= 0 ? Rectangle.Empty : new Rectangle(sx, sy, sw, sh);
                }
            }
        }

        private void EnsureMinimumSize()
        {
            Vector2 rawApprox = new(
                ScaleXNonZero(this.Scale.X) ? this.Size.X / this.Scale.X : this.Size.X,
                ScaleXNonZero(this.Scale.Y) ? this.Size.Y / this.Scale.Y : this.Size.Y
            );

            Vector2 clamped = ClampToMinimumRawSize(rawApprox);

            this.Size = clamped;
        }

        private static bool ScaleXNonZero(float s)
        {
            return Math.Abs(s) > 1e-6f;
        }

        private Vector2 ClampToMinimumRawSize(Vector2 raw)
        {
            float minRawW = this.tileSize.X * 2f;
            float minRawH = this.tileSize.Y * 2f;

            float w = MathF.Max(raw.X, minRawW);
            float h = MathF.Max(raw.Y, minRawH);

            return new Vector2(w, h);
        }
    }
}
