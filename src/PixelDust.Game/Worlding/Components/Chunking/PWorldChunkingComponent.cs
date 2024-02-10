using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants;
using PixelDust.Game.Managers;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.Worlding.Components.Chunking
{
    public sealed class PWorldChunkingComponent : PWorldComponent
    {
        public static short DefaultChunkSize => 6;

        private int worldChunkWidth;
        private int worldChunkHeight;

        private PWorldChunk[,] _chunks;

        private Texture2D pixelTexture;

        protected override void OnAwake()
        {
            this.pixelTexture = this.Game.AssetsDatabase.GetTexture("particle_1");

            this._chunks = new PWorldChunk[this.World.Infos.Size.Width / DefaultChunkSize + 1, this.World.Infos.Size.Height / DefaultChunkSize + 1];

            this.worldChunkWidth = this._chunks.GetLength(0);
            this.worldChunkHeight = this._chunks.GetLength(1);

            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this._chunks[x, y] = new(new(x * DefaultChunkSize * PWorldConstants.GRID_SCALE, y * DefaultChunkSize * PWorldConstants.GRID_SCALE), DefaultChunkSize);
                }
            }
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this._chunks[x, y].Update();
                }
            }
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
#if DEBUG
            // Debug methods
            // DEBUG_DrawActiveChunks(spriteBatch);
#endif
        }

        public bool TryGetChunkUpdateState(Vector2Int pos, out bool result)
        {
            result = false;
            Vector2Int targetPos = ToChunkCoordinateSystem(pos);

            if (!IsWithinChunkBoundaries(targetPos))
            {
                return false;
            }

            result = this._chunks[targetPos.X, targetPos.Y].ShouldUpdate;
            return true;
        }
        public int GetActiveChunksCount()
        {
            int result = 0;
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    if (this._chunks[x, y].ShouldUpdate)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public bool TryNotifyChunk(Vector2Int pos)
        {
            Vector2Int targetPos = ToChunkCoordinateSystem(pos);

            if (IsWithinChunkBoundaries(targetPos))
            {
                this._chunks[targetPos.X, targetPos.Y].Notify();
                TryNotifyNeighboringChunks(pos, targetPos);

                return true;
            }

            return false;
        }
        private void TryNotifyNeighboringChunks(Vector2Int ePos, Vector2Int cPos)
        {
            if (ePos.X % DefaultChunkSize == 0 && IsWithinChunkBoundaries(new(cPos.X - 1, cPos.Y)))
            {
                this._chunks[cPos.X - 1, cPos.Y].Notify();
            }

            if (ePos.X % DefaultChunkSize == DefaultChunkSize - 1 && IsWithinChunkBoundaries(new(cPos.X + 1, cPos.Y)))
            {
                this._chunks[cPos.X + 1, cPos.Y].Notify();
            }

            if (ePos.Y % DefaultChunkSize == 0 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y - 1)))
            {
                this._chunks[cPos.X, cPos.Y - 1].Notify();
            }

            if (ePos.Y % DefaultChunkSize == DefaultChunkSize - 1 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y + 1)))
            {
                this._chunks[cPos.X, cPos.Y + 1].Notify();
            }
        }

        private bool IsWithinChunkBoundaries(Vector2Int pos)
        {
            return pos.X >= 0 && pos.X < this.worldChunkWidth &&
                   pos.Y >= 0 && pos.Y < this.worldChunkHeight;
        }

        public static Vector2Int ToChunkCoordinateSystem(Vector2Int pos)
        {
            return new(pos.X / DefaultChunkSize, pos.Y / DefaultChunkSize);
        }

#if DEBUG
        private void DEBUG_DrawActiveChunks(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    if (this._chunks[x, y].ShouldUpdate)
                    {
                        spriteBatch.Draw(this.pixelTexture, new Vector2(this._chunks[x, y].Position.X, this._chunks[x, y].Position.Y), null, new Color(255, 0, 0, 35), 0f, Vector2.Zero, DefaultChunkSize * PWorldConstants.GRID_SCALE, SpriteEffects.None, 0f);
                    }
                }
            }
        }
#endif
    }
}
#pragma warning restore IDE0051